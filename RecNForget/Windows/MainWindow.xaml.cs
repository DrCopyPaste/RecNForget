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
using RecNForget.Control.Services;
using RecNForget.Controls;
using RecNForget.Controls.Extensions;
using RecNForget.Services;
using RecNForget.Services.Contracts;

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

        private string replayStartAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "playbackStart.wav");
        private string replayStopAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "playbackStop.wav");
        private string recordStartAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "startRec.wav");
        private string recordStopAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "stopRec.wav");
        private AudioRecordingService audioRecordingService = null;
        private IActionService actionService = null;
        private HotkeyService hotkeyService = null;
        private IAppSettingService settingService = null;
        private IAudioPlaybackService audioPlaybackService = null;
        private ISelectedFileService selectedFileService = null;

        private string taskBar_ProgressState = "None";

        private DispatcherTimer recordingTimer;

        private string recordingTimeimeDisplay = string.Empty;
        private string currentFileNameDisplay;

        public MainWindow(IAppSettingService appSettingService, ISelectedFileService selectedFileService, IAudioPlaybackService audioPlaybackService)
        {
            DataContext = this;
            InitializeComponent();

            SettingService = appSettingService;
            currentVersion = new ApplicationBase();

            // ensure AppConfig Values exist
            bool firstTimeUser = SettingService.RestoreDefaultAppConfigSetting(settingKey: null, overrideSetting: false);
            Version lastInstalledVersion = SettingService.LastInstalledVersion;
            SettingService.LastInstalledVersion = new Version(ThisAssembly.AssemblyFileVersion);

            this.actionService = new ActionService();
            SelectedFileService = selectedFileService;
            SelectedFileService.SelectLatestFile();

            AudioPlaybackService = audioPlaybackService;
            AudioPlaybackService.PropertyChanged += AudioPlaybackService_PropertyChanged;

            recordingTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 30)
            };

            recordingTimer.Tick += new EventHandler(RecordingTimer_Tick);

            AudioRecordingService = new AudioRecordingService(appSettingService: settingService);
            AudioRecordingService.PropertyChanged += AudioRecordingService_PropertyChanged;

            this.hotkeyService = new HotkeyService();
            this.hotkeyService.AddHotkey(
                () => { return HotkeySettingTranslator.GetHotkeySettingAsString(SettingService.HotKey_StartStopRecording); },
                () => { if (AudioPlaybackService.Stopped) { AudioRecordingService.ToggleRecording(); } });        
            
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
                var dia = new NewToApplicationWindow(this.hotkeyService, SettingService);

                if (!SettingService.MinimizedToTray)
                {
                    dia.Owner = this;
                }

                dia.Show();
            }
            else if (currentVersion.Info.Version.CompareTo(lastInstalledVersion) > 0)
            {
                var newToVersionDialog = new NewToVersionDialog(lastInstalledVersion, currentVersion.Info.Version, SettingService);

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

        ~MainWindow()
        {
            AudioPlaybackService.PropertyChanged -= AudioPlaybackService_PropertyChanged;
            AudioRecordingService.PropertyChanged -= AudioRecordingService_PropertyChanged;
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

        public IAppSettingService SettingService
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

        public IAudioPlaybackService AudioPlaybackService
        {
            get
            {
                return audioPlaybackService;
            }

            set
            {
                audioPlaybackService = value;
                OnPropertyChanged();
            }
        }

        public ISelectedFileService SelectedFileService
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

        private void AudioRecordingService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AudioRecordingService.CurrentlyRecording):
                {
                    if (AudioRecordingService.CurrentlyRecording)
                    {
                        if (SettingService.PlayAudioFeedBackMarkingStartAndStopRecording)
                        {
                            audioPlaybackService.KillAudio(reset: true);

                            audioPlaybackService.QueueFile(recordStartAudioFeedbackPath);
                            audioPlaybackService.Play();

                            while (audioPlaybackService.PlaybackState != PlaybackState.Stopped) { }

                            audioPlaybackService.KillAudio(reset: true);
                        }

                        if (SettingService.ShowBalloonTipsForRecording)
                        {
                            trayIcon.ShowBalloonTip(balloonTipTimeout, "Recording started!", "RecNForget now recording...", ToolTipIcon.Info);
                            trayIcon.BalloonTipClicked -= TaskBarIcon_TrayBalloonTipClicked;
                        }

                        RecordButton.Style = (Style)FindResource("StopRecordButton");
                        AudioPlaybackService.KillAudio(reset: true);
                        TaskBar_ProgressState = "Error";
                        RecordingStart = DateTime.Now;
                        recordingTimer.Start();
                        UpdateCurrentFileNameDisplay();
                    }
                    else
                    {
                        if (SettingService.PlayAudioFeedBackMarkingStartAndStopRecording || SettingService.AutoReplayAudioAfterRecording)
                        {
                            if (SettingService.PlayAudioFeedBackMarkingStartAndStopRecording)
                            {
                                QueueAudioPlayback(fileName: recordStopAudioFeedbackPath);
                            }

                            if (SettingService.AutoReplayAudioAfterRecording)
                            {
                                QueueAudioPlayback(
                                    fileName: AudioRecordingService.LastFileName,
                                    startIndicatorFileName: SettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? replayStartAudioFeedbackPath : null,
                                    endIndicatorFileName: SettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? replayStopAudioFeedbackPath : null);
                            }

                            ToggleReplayLastRecording();
                        }

                        RecordButton.Style = (Style)FindResource("RecordButton");
                        UpdateCurrentFileNameDisplay(reset: true);
                        TaskBar_ProgressState = "None";
                        recordingTimer.Stop();

                        if (SettingService.ShowBalloonTipsForRecording)
                        {
                            trayIcon.ShowBalloonTip(balloonTipTimeout, "Recording saved!", AudioRecordingService.LastFileName, ToolTipIcon.Info);
                            trayIcon.BalloonTipClicked += TaskBarIcon_TrayBalloonTipClicked;
                        }

                        if (SettingService.AutoSelectLastRecording)
                        {
                            SelectedFileService.SelectFile(new FileInfo(AudioRecordingService.LastFileName));
                        }
                    }

                    break;
                }
            }
        }

        private void AudioPlaybackService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AudioPlaybackService.Paused):
                {
                    // ToDo better way to distinguish, whether AudioPlaybackService is currently playing a "file" or "just" giving feedback from recording start/stop
                    TogglePlaySelectedFileButton.Style = AudioPlaybackService.Playing && !recordingTimer.IsEnabled ? (Style)FindResource("PauseButton") : (Style)FindResource("PlayButton");
                    break;
                }

                case nameof(AudioPlaybackService.Stopped):
                {
                    TaskBar_ProgressState = AudioPlaybackService.Stopped || AudioRecordingService.CurrentlyRecording ? "None" : "Normal";
                    break;
                }
            }
        }

        private void ShowRandomApplicationTip()
        {
            var quickTip = new QuickTipDialog(SettingService);

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
            // ToDo these keys should be configurable
            // ensure not triggering any of these if hotkey to record is the same

            var recHotkey = HotkeySettingTranslator.GetHotkeySettingAsList(SettingService.HotKey_StartStopRecording, string.Empty, string.Empty);

            if (!recHotkey.Contains(e.Key.ToString()))
            {
                if (e.Key == Key.Return)
                {
                    actionService.ChangeSelectedFileName(this);
                }
                else if (e.Key == Key.Delete)
                {
                    actionService.DeleteSelectedFile(this);
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
                        if (AudioPlaybackService.ItemsCount == 0)
                        {
                            QueueAudioPlayback(
                                fileName: SelectedFileService.SelectedFile.FullName,
                                startIndicatorFileName: SettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? replayStartAudioFeedbackPath : null,
                                endIndicatorFileName: SettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? replayStopAudioFeedbackPath : null);
                        }

                        ToggleReplayLastRecording();
                    }
                }
                else if (e.Key == Key.Escape)
                {
                    StopReplayLastRecordingButton.PerformClick();
                }
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
            AudioPlaybackService.Stop();
        }

        private void SkipPrevButton_Click(object sender, RoutedEventArgs e)
        {
            AudioPlaybackService.Stop();
            SelectedFileService.SelectPrevFile();
        }

        private void SkipNextButton_Click(object sender, RoutedEventArgs e)
        {
            AudioPlaybackService.Stop();
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

                        if (!SettingService.MinimizedToTray)
                        {
                            installUpdateDialog.Owner = Window.GetWindow(this);
                        }

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

                            if (!SettingService.MinimizedToTray)
                            {
                                tempDialog.Owner = Window.GetWindow(this);
                            }

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

                        if (!SettingService.MinimizedToTray)
                        {
                            errorDialog.Owner = Window.GetWindow(this);
                        }

                        errorDialog.ShowDialog();
                    });
                }
            }
        }

        private void ReplayLastRecording_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlaybackService.ItemsCount == 0)
            {
                QueueAudioPlayback(
                    fileName: SelectedFileService.SelectedFile.FullName,
                    startIndicatorFileName: SettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? replayStartAudioFeedbackPath : null,
                    endIndicatorFileName: SettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? replayStopAudioFeedbackPath : null);
            }

            ToggleReplayLastRecording();
        }

        private bool QueueAudioPlayback(string fileName = null, string startIndicatorFileName = null, string endIndicatorFileName = null)
        {
            bool replayFileExists = false;
            string fileNameToPlay;

            if (fileName == null)
            {
                replayFileExists = SelectedFileService.SelectedFile.Exists;
                fileNameToPlay = SelectedFileService.SelectedFile.FullName;
            }
            else
            {
                var fileInfo = new FileInfo(fileName);
                replayFileExists = fileInfo.Exists;
                fileNameToPlay = fileName;
            }

            if (!replayFileExists)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(startIndicatorFileName))
            {
                AudioPlaybackService.QueueFile(startIndicatorFileName);
            }

            AudioPlaybackService.QueueFile(fileNameToPlay);

            if (!string.IsNullOrEmpty(endIndicatorFileName))
            {
                AudioPlaybackService.QueueFile(endIndicatorFileName);
            }

            return true;
        }

        private void ToggleReplayLastRecording()
        {
            if (AudioPlaybackService.PlaybackState == PlaybackState.Stopped)
            {
                AudioPlaybackService.Play();
            }
            else if (AudioPlaybackService.PlaybackState == PlaybackState.Playing)
            {
                AudioPlaybackService.Pause();
            }
            else if (AudioPlaybackService.PlaybackState == PlaybackState.Paused)
            {
                AudioPlaybackService.Play();
            }
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
            var settingsWindow = new SettingsWindow(hotkeyService, SettingService);

            if (!SettingService.MinimizedToTray)
            {
                settingsWindow.Owner = this;
            }

            settingsWindow.ShowDialog();

            UpdateCurrentFileNameDisplay();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var aboutDialog = new AboutDialog(SettingService);

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
    }
}