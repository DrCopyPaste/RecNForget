using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.VisualBasic.ApplicationServices;
using NAudio.Wave;
using Ookii.Dialogs.Wpf;
using RecNForget.Controls;
using RecNForget.Services;
using RecNForget.Services.Extensions;

namespace RecNForget.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private NotifyIcon trayIcon;
        private int balloonTipTimeout = 3000;

        private ApplicationBase currentVersion;

        private string recordStartAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "startRec.wav");
        private string recordStopAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "stopRec.wav");

        private string replayStartAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "playbackStart.wav");
        private string replayStopAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "playbackStop.wav");

        private AudioPlayListService recordingFeedbackAudioService = null;
        private AudioRecordingService audioRecordingService = null;
        private HotkeyService hotkeyService = null;
        private AppSettingService settingService = null;
        private AudioPlayListService replayAudioService = null;

        private string taskBar_ProgressState = "None";

        private DispatcherTimer recordingTimer;

        private string recordingTimeimeDisplay = string.Empty;
        private string currentFileNameDisplay;

        private SelectedFileService selectedFileService;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            this.SettingService = new AppSettingService();
            currentVersion = new ApplicationBase();

            // ensure AppConfig Values exist
            bool firstTimeUser = SettingService.RestoreDefaultAppConfigSetting(settingKey: null, overrideSetting: false);
            Version lastInstalledVersion = SettingService.LastInstalledVersion;
            SettingService.LastInstalledVersion = new Version(ThisAssembly.AssemblyFileVersion);

            SelectedFileService = new SelectedFileService();
            SelectedFileService.SelectLatestFile();

            ReplayAudioService = new AudioPlayListService(
                beforePlayAction: () =>
                {
                    TogglePlaySelectedFileButton.Style = (Style)FindResource("PauseButton");
                    TaskBar_ProgressState = "Normal";
                },
                afterStopAction: () =>
                {
                    TogglePlaySelectedFileButton.Style = (Style)FindResource("PlayButton");
                    ReplayAudioService.KillAudio(reset: true);
                    TaskBar_ProgressState = "None";
                },
                afterPauseAction: () =>
                {
                    TogglePlaySelectedFileButton.Style = (Style)FindResource("PlayButton");
                },
                afterResumeAction: () =>
                {
                    TogglePlaySelectedFileButton.Style = (Style)FindResource("PauseButton");
                });

            recordingFeedbackAudioService = new AudioPlayListService();

            recordingTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 30)
            };

            recordingTimer.Tick += new EventHandler(RecordingTimer_Tick);

            AudioRecordingService = new AudioRecordingService(
                startRecordingAction: () =>
                {
                    RecordButton.Style = (Style)FindResource("StopRecordButton");
                    ReplayAudioService.KillAudio(reset: true);
                    TaskBar_ProgressState = "Error";
                    RecordingStart = DateTime.Now;
                    recordingTimer.Start();
                    UpdateCurrentFileNameDisplay();

                    if (SettingService.ShowBalloonTipsForRecording)
                    {
                        trayIcon.ShowBalloonTip(balloonTipTimeout, "Recording started!", "RecNForget now recording...", ToolTipIcon.Info);
                        trayIcon.BalloonTipClicked -= TaskBarIcon_TrayBalloonTipClicked;
                    }

                    if (SettingService.PlayAudioFeedBackMarkingStartAndStopRecording)
                    {
                        PlayRecordingStartAudioFeedback();
                    }
                },
                stopRecordingAction: () =>
                {
                    RecordButton.Style = (Style)FindResource("RecordButton");
                    UpdateCurrentFileNameDisplay(reset: true);
                    TaskBar_ProgressState = "None";
                    if (SettingService.ShowBalloonTipsForRecording)
                    {
                        trayIcon.ShowBalloonTip(balloonTipTimeout, "Recording saved!", AudioRecordingService.LastFileName, ToolTipIcon.Info);
                        trayIcon.BalloonTipClicked += TaskBarIcon_TrayBalloonTipClicked;
                    }

                    recordingTimer.Stop();
                    if (SettingService.PlayAudioFeedBackMarkingStartAndStopRecording)
                    {
                        PlayRecordingStopAudioFeedback();
                    }

                    if (SettingService.AutoSelectLastRecording)
                    {
                        SelectedFileService.SelectFile(new FileInfo(AudioRecordingService.LastFileName));
                    }

                    if (SettingService.AutoReplayAudioAfterRecording)
                    {
                        ToggleReplayLastRecording(AudioRecordingService.LastFileName);
                    }
                },
                outputPathGetterMethod: () =>
                {
                    return SettingService.OutputPath;
                },
                filenamePrefixGetterMethod: () =>
                {
                    return SettingService.FilenamePrefix;
                });

            this.hotkeyService = new HotkeyService();
            this.hotkeyService.AddHotkey(
                () => { return HotkeySettingTranslator.GetHotkeySettingAsString(SettingService.HotKey_StartStopRecording); },
                () => { if (ReplayAudioService.Stopped) { AudioRecordingService.ToggleRecording(); } });        
            
            this.KeyDown += Window_KeyDown;

            // Workaround: binding to main window properties when session started "minimized to tray" does not work
            AlwaysOnTopMenuEntry.IsChecked = SettingService.WindowAlwaysOnTop;
            MinimizedToTrayMenuEntry.IsChecked = SettingService.MinimizedToTray;
            OutputPathControlVisibilityMenuEntry.IsChecked = SettingService.OutputPathControlVisible;
            SelectedFileControlVisibilityMenuEntry.IsChecked = SettingService.SelectedFileControlVisible;

            OutputPathControl.Visibility = SettingService.OutputPathControlVisible ? Visibility.Visible : Visibility.Collapsed;
            SelectedFileControl.Visibility = SettingService.SelectedFileControlVisible ? Visibility.Visible : Visibility.Collapsed;

            if (SettingService.CheckForUpdateOnStart)
            {
                Task.Run(() => { CheckForUpdates(); });
            }

            this.Topmost = SettingService.WindowAlwaysOnTop;

            trayIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Visible = true
            };

            trayIcon.Click += new EventHandler(TrayIcon_Click);
            trayIcon.DoubleClick += new EventHandler(TrayIconMenu_DoubleClick);

            UpdateCurrentFileNameDisplay(reset: true);

            if (SettingService.MinimizedToTray)
            {
                SwitchToBackgroundMode();
            }
            else
            {
                SwitchToForegroundMode();
            }

            if (firstTimeUser)
            {
                var dia = new NewToApplicationWindow(this.hotkeyService);

                if (!SettingService.MinimizedToTray)
                {
                    dia.Owner = this;
                }

                dia.Show();
            }
            else if (currentVersion.Info.Version.CompareTo(lastInstalledVersion) > 0)
            {
                var newToVersionDialog = new NewToVersionDialog(lastInstalledVersion, currentVersion.Info.Version);

                if (!SettingService.MinimizedToTray)
                {
                    newToVersionDialog.Owner = this;
                }

                newToVersionDialog.Show();
            }
            else if (SettingService.ShowTipsAtApplicationStart)
            {
                ShowRandomApplicationTip();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AudioRecordingService AudioRecordingService
        {
            get
            {
                return audioRecordingService;
            }

            set
            {
                audioRecordingService = value;
                OnPropertyChanged();
            }
        }

        public AppSettingService SettingService
        {
            get
            {
                return settingService;
            }

            set
            {
                settingService = value;
                OnPropertyChanged();
            }
        }

        public AudioPlayListService ReplayAudioService
        {
            get
            {
                return replayAudioService;
            }

            set
            {
                replayAudioService = value;
                OnPropertyChanged();
            }
        }

        public SelectedFileService SelectedFileService
        {
            get
            {
                return selectedFileService;
            }

            set
            {
                selectedFileService = value;
                OnPropertyChanged();
            }
        }

        public DateTime RecordingStart { get; set; }

        public string RecordingTimeDisplay
        {
            get
            {
                return AudioRecordingService.CurrentlyRecording ? recordingTimeimeDisplay : string.Empty;
            }

            set
            {
                recordingTimeimeDisplay = value;
                OnPropertyChanged();
            }
        }

        public string TaskBar_ProgressState
        {
            get
            {
                return taskBar_ProgressState;
            }

            set
            {
                taskBar_ProgressState = value;
                OnPropertyChanged();
            }
        }

        public string CurrentFileNameDisplay
        {
            get
            {
                return currentFileNameDisplay;
            }

            set
            {
                currentFileNameDisplay = value;
                OnPropertyChanged();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            SettingService.MainWindowLeftX = this.Left;
            SettingService.MainWindowTopY = this.Top;

            if (SettingService.MinimizedToTray && this.IsVisible)
            {
                var closeOrBackgroundDialog = new CloseOrBackgroundDialog()
                {
                    Owner = this
                };

                var dialogResult = closeOrBackgroundDialog.ShowDialog();

                if (dialogResult.HasValue && dialogResult.Value)
                {
                    e.Cancel = true;
                    SwitchToBackgroundMode();
                }
            }
        }

        private void ShowRandomApplicationTip()
        {
            var quickTip = new QuickTipDialog();

            if (!SettingService.MinimizedToTray)
            {
                quickTip.Owner = this;
            }

            quickTip.Show();
        }
               
        private void WindowOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenSettingsMenu();

            e.Handled = true;
        }

        private void TaskBarIcon_TrayBalloonTipClicked(object sender, EventArgs e)
        {
            if (AudioRecordingService.LastFileName == string.Empty || !File.Exists(AudioRecordingService.LastFileName))
            {
                return;
            }

            string argument = "/select, \"" + AudioRecordingService.LastFileName + "\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        private void MainWindow_MouseEnter(object sender, EventArgs e)
        {
            TitleBar.Visibility = Visibility.Visible;
        }

        // ToDo: why does this trigger after drag move? Is there a way around it?
        private void MainWindow_MouseLeave(object sender, EventArgs e)
        {
            TitleBar.Visibility = Visibility.Hidden;
        }

        private void ToggleOutputPathControlVisibility(object sender, EventArgs e)
        {
            SettingService.OutputPathControlVisible = !SettingService.OutputPathControlVisible;
            OutputPathControl.Visibility = SettingService.OutputPathControlVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ToggleSelectedFileControlVisibility(object sender, EventArgs e)
        {
            SettingService.SelectedFileControlVisible = !SettingService.SelectedFileControlVisible;
            SelectedFileControl.Visibility = SettingService.SelectedFileControlVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ToggleAlwaysOnTop(object sender, EventArgs e)
        {
            SettingService.WindowAlwaysOnTop = !SettingService.WindowAlwaysOnTop;
            this.Topmost = SettingService.WindowAlwaysOnTop;
        }

        private void ToggleMinimizedToTray(object sender, EventArgs e)
        {
            SettingService.MinimizedToTray = !SettingService.MinimizedToTray;

            if (SettingService.MinimizedToTray)
            {
                SwitchToBackgroundMode();
            }
            else
            {
                SwitchToForegroundMode();
            }
        }

        private void SwitchToBackgroundMode()
        {
            this.Hide();
            trayIcon.ShowBalloonTip(balloonTipTimeout, "Running in background now!", @"RecNForget is now running in the background. Double click tray icon to restore", ToolTipIcon.Info);
        }

        private void SwitchToForegroundMode()
        {
            this.Show();

            // try restore last window positon
            if (!SettingService.MainWindowLeftX.HasValue || !SettingService.MainWindowTopY.HasValue)
            {
                this.Left = (SystemParameters.PrimaryScreenWidth / 2) - (this.Width / 2);
                this.Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);
            }
            else
            {
                this.Left = SettingService.MainWindowLeftX.Value;
                this.Top = SettingService.MainWindowTopY.Value;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FilenamePrefix_Changed(object sender, RoutedEventArgs e)
        {
            // https://stackoverflow.com/a/23182807
            SettingService.FilenamePrefix = string.Concat(SettingService.FilenamePrefix.Split(Path.GetInvalidFileNameChars()));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            if ((e as System.Windows.Forms.MouseEventArgs).Button == MouseButtons.Right)
            {
                OpenSettingsMenu();
            }
        }

        private void TrayIconMenu_DoubleClick(object sender, EventArgs e)
        {
            SwitchToForegroundMode();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ChangeSelectedFileNameButton.PerformClick();
            }
            else if (e.Key == Key.Delete)
            {
                DeleteSelectedFileButton.PerformClick();
            }
            else if (e.Key == Key.Down)
            {
                OpenSelectedFileButton.PerformClick();
            }
            else if (e.Key == Key.Left)
            {
                SkipPrevButton.PerformClick();
            }
            else if (e.Key == Key.Right)
            {
                SkipNextButton.PerformClick();
            }
            else if (e.Key == Key.Space)
            {
                if (SelectedFileService.HasSelectedFile && AudioRecordingService.CurrentlyNotRecording)
                {
                    ToggleReplayLastRecording();
                }
            }
            else if (e.Key == Key.Escape)
            {
                StopReplayLastRecordingButton.PerformClick();
            }
        }

        private void RecordingTimer_Tick(object sender, EventArgs e)
        {
            RecordingTimeDisplay = TimeSpan.FromMilliseconds(DateTime.Now.Subtract(RecordingStart).TotalMilliseconds).TotalSeconds.ToString(@"0.##");
            UpdateCurrentFileNameDisplay();
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            AudioRecordingService.ToggleRecording();
        }

        private void UpdateCurrentFileNameDisplay(bool reset = false)
        {
            CurrentFileNameDisplay = !AudioRecordingService.CurrentlyRecording || reset || hotkeyService == null ?
                AudioRecordingService.GetTargetPathTemplateString() :
                string.Format("{0} ({1} s)", AudioRecordingService.CurrentFileName, RecordingTimeDisplay);
        }

        private void OpenOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            var directory = new DirectoryInfo(SettingService.OutputPath);

            if (SelectedFileService.HasSelectedFile && SelectedFileService.SelectedFile.Exists)
            {
                // if there is a result select it in an explorer window
                string argument = "/select, \"" + SelectedFileService.SelectedFile.FullName + "\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
            else
            {
                if (!directory.Exists)
                {
                    directory.Create();
                }

                // otherwise just open output path in explorer
                Process.Start(SettingService.OutputPath);
            }
        }

        private void StopReplayLastRecording_Click(object sender, RoutedEventArgs e)
        {
            ReplayAudioService.Stop();
            ReplayAudioService.KillAudio(reset: true);
        }

        private void SkipPrevButton_Click(object sender, RoutedEventArgs e)
        {
            ReplayAudioService.Stop();
            ReplayAudioService.KillAudio(reset: true);
            SelectedFileService.SelectPrevFile();
        }

        private void SkipNextButton_Click(object sender, RoutedEventArgs e)
        {
            ReplayAudioService.Stop();
            ReplayAudioService.KillAudio(reset: true);
            SelectedFileService.SelectNextFile();
        }

        private async void CheckForUpdates(bool showMessages = false)
        {
            try
            {
                var newerReleases = await UpdateChecker.GetNewerReleases(oldVersionString: ThisAssembly.AssemblyFileVersion);

                if (newerReleases.Any())
                {
                    string changeLog = UpdateChecker.GetAllChangeLogs(newerReleases);

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        var installUpdateDialog = new ReleaseInstallationDialog(newerReleases.First(), UpdateChecker.GetValidVersionStringMsiAsset(newerReleases.First()), changeLog);
                        installUpdateDialog.ShowDialog();
                    });
                }
                else
                {
                    if (showMessages)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            CustomMessageBox tempDialog = new CustomMessageBox(
                                caption: "RecNForget is already up to date!",
                                icon: CustomMessageBoxIcon.Information,
                                buttons: CustomMessageBoxButtons.OK,
                                messageRows: new List<string>() { "No newer version found" },
                                controlFocus: CustomMessageBoxFocus.Ok);

                            tempDialog.ShowDialog();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                if (showMessages)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        var errorDialog = new CustomMessageBox(
                            caption: "Error during update",
                            icon: CustomMessageBoxIcon.Error,
                            buttons: CustomMessageBoxButtons.OK,
                            messageRows: new List<string>() { "An error occurred trying to get updates:", ex.InnerException.Message },
                            controlFocus: CustomMessageBoxFocus.Ok);

                        errorDialog.ShowDialog();
                    });
                }
            }
        }

        private void ReplayLastRecording_Click(object sender, RoutedEventArgs e)
        {
            ToggleReplayLastRecording();
        }

        // https://github.com/naudio/NAudio/blob/master/Docs/PlayAudioFileWinForms.md
        private void ToggleReplayLastRecording(string fileName = null)
        {
            bool replayFileExists = SelectedFileService.SelectedFile.Exists;

            if (ReplayAudioService.PlaybackState == PlaybackState.Stopped)
            {
                if (SettingService.PlayAudioFeedBackMarkingStartAndStopReplaying)
                {
                    ReplayAudioService.QueueFile(replayStartAudioFeedbackPath);
                }

                ReplayAudioService.QueueFile(fileName ?? SelectedFileService.SelectedFile.FullName);

                if (SettingService.PlayAudioFeedBackMarkingStartAndStopReplaying)
                {
                    ReplayAudioService.QueueFile(replayStopAudioFeedbackPath);
                }

                if (replayFileExists)
                {
                    ReplayAudioService.Play();
                }
                else
                {
                    SelectedFileService.SelectFile(null);

                    System.Windows.MessageBox.Show("The last recorded audio file has been moved or deleted.", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (ReplayAudioService.PlaybackState == PlaybackState.Playing)
            {
                ReplayAudioService.Pause();
            }
            else if (ReplayAudioService.PlaybackState == PlaybackState.Paused)
            {
                ReplayAudioService.Play();
            }
        }

        private void PlayRecordingStartAudioFeedback()
        {
            recordingFeedbackAudioService.QueueFile(recordStartAudioFeedbackPath);
            recordingFeedbackAudioService.Play();

            while (recordingFeedbackAudioService.PlaybackState == PlaybackState.Playing) { }

            recordingFeedbackAudioService.KillAudio(reset: true);
        }

        private void PlayRecordingStopAudioFeedback()
        {
            recordingFeedbackAudioService.QueueFile(recordStopAudioFeedbackPath);
            recordingFeedbackAudioService.Play();

            while (recordingFeedbackAudioService.PlaybackState == PlaybackState.Playing) { }

            recordingFeedbackAudioService.KillAudio(reset: true);
        }

        private void OpenSettingsMenu()
        {
            WindowOptionsMenu.IsOpen = true;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            var helpmenu = new HelpWindow();

            if (!SettingService.MinimizedToTray)
            {
                helpmenu.Owner = this;
            }

            helpmenu.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(hotkeyService);

            if (!SettingService.MinimizedToTray)
            {
                settingsWindow.Owner = this;
            }

            settingsWindow.ShowDialog();

            UpdateCurrentFileNameDisplay();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var aboutDialog = new AboutDialog();

            if (!SettingService.MinimizedToTray)
            {
                aboutDialog.Owner = this;
            }

            aboutDialog.ShowDialog();
        }

        private void CheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { CheckForUpdates(showMessages: true); });
        }

        private void ChangeFileNamePatternButton_Clicked(object sender, RoutedEventArgs e)
        {
            CustomMessageBox tempDialog = new CustomMessageBox(
                caption: "Type in a new pattern for file name generation.",
                icon: CustomMessageBoxIcon.Question,
                buttons: CustomMessageBoxButtons.OkAndCancel,
                messageRows: new List<string>() { "Supported placeholders:", "(Date), (Guid)", "If you do not provide a placeholder to create unique file names, RecNForget will do it for you." },
                prompt: SettingService.FilenamePrefix,
                controlFocus: CustomMessageBoxFocus.Prompt,
                promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

            if (!SettingService.MinimizedToTray)
            {
                tempDialog.Owner = this;
            }

            if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
            {
                SettingService.FilenamePrefix = tempDialog.PromptContent;
                UpdateCurrentFileNameDisplay();
            }
        }

        private void ChangeOutputFolderButton_Clicked(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();

            if (dialog.ShowDialog(this) == true)
            {
                SettingService.OutputPath = dialog.SelectedPath;
                UpdateCurrentFileNameDisplay();
                SelectedFileService.SelectLatestFile();
            }
        }

        private void ChangeSelectedFileNameButton_Clicked(object sender, RoutedEventArgs e)
        {
            ReplayAudioService.Stop();
            ReplayAudioService.KillAudio();

            CustomMessageBox tempDialog = new CustomMessageBox(
                caption: "Rename the selected file",
                icon: CustomMessageBoxIcon.Question,
                buttons: CustomMessageBoxButtons.OkAndCancel,
                messageRows: new List<string>(),
                prompt: Path.GetFileNameWithoutExtension(selectedFileService.SelectedFile.Name),
                controlFocus: CustomMessageBoxFocus.Prompt,
                promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

            if (!SettingService.MinimizedToTray)
            {
                tempDialog.Owner = this;
            }

            if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
            {
                if (!selectedFileService.RenameSelectedFileWithoutExtension(tempDialog.PromptContent))
                {
                    CustomMessageBox errorMessageBox = new CustomMessageBox(
                        caption: "Something went wrong",
                        icon: CustomMessageBoxIcon.Error,
                        buttons: CustomMessageBoxButtons.OK,
                        messageRows: new List<string>() { "An error occurred trying to rename the selected file" },
                        controlFocus: CustomMessageBoxFocus.Ok);

                    errorMessageBox.ShowDialog();
                }
            }
        }

        private void DeleteSelectedFileButton_Clicked(object sender, RoutedEventArgs e)
        {
            ReplayAudioService.Stop();
            ReplayAudioService.KillAudio();

            CustomMessageBox tempDialog = new CustomMessageBox(
                caption: "Are you sure you want to delete this file?",
                icon: CustomMessageBoxIcon.Question,
                buttons: CustomMessageBoxButtons.OkAndCancel,
                messageRows: new List<string>() { selectedFileService.SelectedFile.FullName },
                controlFocus: CustomMessageBoxFocus.Ok);

            if (!SettingService.MinimizedToTray)
            {
                tempDialog.Owner = this;
            }

            if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
            {
                if (!SelectedFileService.DeleteSelectedFile())
                {
                    CustomMessageBox errorMessageBox = new CustomMessageBox(
                        caption: "Something went wrong",
                        icon: CustomMessageBoxIcon.Error,
                        buttons: CustomMessageBoxButtons.OK,
                        messageRows: new List<string>() { "An error occurred trying to delete the selected file" },
                        controlFocus: CustomMessageBoxFocus.Ok);

                    errorMessageBox.ShowDialog();
                }
            }
        }
    }
}