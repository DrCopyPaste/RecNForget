using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using NAudio.Wave;
using Notifications.Wpf.Core;
using RecNForget.Controls.IoC;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.WPF.Services.Contracts;
using Unity;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private NotifyIcon trayIcon;

        private IAudioRecordingService audioRecordingService = null;
        private IActionService actionService = null;
        private IApplicationHotkeyService hotkeyService = null;
        private IAppSettingService settingService = null;
        private IAudioPlaybackService audioPlaybackService = null;
        private ISelectedFileService selectedFileService = null;

        private string taskBar_ProgressState = "None";

        private readonly NotificationManager _notificationManager = new NotificationManager();

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();


            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.actionService = new DesignerActionService();
                this.hotkeyService = new DesignerApplicationHotkeyService();
                SelectedFileService = new DesignerSelectedFileService();
                SettingService = new DesignerAppSettingService();
                AudioRecordingService = new DesignerAudioRecordingService();
                AudioPlaybackService = new DesignerAudioPlaybackService();
                return;
            }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;

                this.actionService = UnityHandler.UnityContainer.Resolve<IActionService>();
                //actionService.ChangeTheme("Simple_Black");
                //this.InvalidateVisual();


                this.hotkeyService = UnityHandler.UnityContainer.Resolve<IApplicationHotkeyService>();

                SelectedFileService = UnityHandler.UnityContainer.Resolve<ISelectedFileService>();

                SettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
                SettingService.PropertyChanged += SettingService_PropertyChanged;

                AudioRecordingService = UnityHandler.UnityContainer.Resolve<IAudioRecordingService>();
                AudioRecordingService.PropertyChanged += AudioRecordingService_PropertyChanged;

                AudioPlaybackService = UnityHandler.UnityContainer.Resolve<IAudioPlaybackService>();
                AudioPlaybackService.PropertyChanged += AudioPlaybackService_PropertyChanged;

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

                this.KeyDown += Window_KeyDown;
                this.MouseRightButtonUp += MainWindow_MouseRightButtonUp;

                trayIcon = new System.Windows.Forms.NotifyIcon
                {
                    Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName),
                    Visible = true
                };

                trayIcon.Click += new EventHandler(TrayIcon_Click);
                trayIcon.DoubleClick += new EventHandler(TrayIconMenu_DoubleClick);

                // initialize control visibility (is being toggled via SettingService_PropertyChanged - binding with bool to visibility converter did not update)
                OutputPathControl.Visibility = SettingService.OutputPathControlVisible ? Visibility.Visible : Visibility.Collapsed;
                OutputPathControlSpacer.Visibility = SettingService.OutputPathControlVisible ? Visibility.Visible : Visibility.Collapsed;
                SelectedFileControl.Visibility = SettingService.SelectedFileControlVisible ? Visibility.Visible : Visibility.Collapsed;
                SelectedFileControlSpacer.Visibility = SettingService.SelectedFileControlVisible ? Visibility.Visible : Visibility.Collapsed;
                RecordingTimerControl.Visibility = SettingService.RecordingTimerControlVisible ? Visibility.Visible : Visibility.Collapsed;
                RecordingTimerControlSpacer.Visibility = SettingService.RecordingTimerControlVisible ? Visibility.Visible : Visibility.Collapsed;

                this.Topmost = SettingService.WindowAlwaysOnTop;


                // hacky way to have a window for NotificationManager
                // is this really needed? (window is loaded into memory even if running in background)
                this.Show();
                if (SettingService.MinimizedToTray)
                {
                    SwitchToBackgroundMode();
                }


                SelectedFileService.SelectLatestFile();
                //else
                //{
                //    SwitchToForegroundMode();
                //}
            }
        }

        ~MainWindow()
        {
            SettingService.PropertyChanged -= SettingService_PropertyChanged;
            AudioPlaybackService.PropertyChanged -= AudioPlaybackService_PropertyChanged;
            AudioRecordingService.PropertyChanged -= AudioRecordingService_PropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IAudioRecordingService AudioRecordingService
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

        protected override void OnClosing(CancelEventArgs e)
        {
            SettingService.MainWindowLeftX = this.Left;
            SettingService.MainWindowTopY = this.Top;
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

                            audioPlaybackService.QueueFile(audioPlaybackService.RecordStartAudioFeedbackPath);
                            audioPlaybackService.Play();

                            while (audioPlaybackService.PlaybackState != PlaybackState.Stopped) { }

                            audioPlaybackService.KillAudio(reset: true);
                        }

                        if (SettingService.ShowBalloonTipsForRecording)
                        {
                            _notificationManager.ShowAsync(
                                new NotificationContent()
                                {
                                    Type = NotificationType.Information,
                                    Title = "Recording started!",
                                    Message = "RecNForget now recording..."
                                });
                        }

                        AudioPlaybackService.KillAudio(reset: true);
                        TaskBar_ProgressState = "Error";
                    }
                    else
                    {
                        if (SettingService.PlayAudioFeedBackMarkingStartAndStopRecording || SettingService.AutoReplayAudioAfterRecording)
                        {
                            if (SettingService.PlayAudioFeedBackMarkingStartAndStopRecording)
                            {
                                actionService.QueueAudioPlayback(fileName: audioPlaybackService.RecordStopAudioFeedbackPath);
                            }

                            if (SettingService.AutoReplayAudioAfterRecording)
                            {
                                actionService.QueueAudioPlayback(
                                    fileName: AudioRecordingService.LastFileName,
                                    startIndicatorFileName: SettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? audioPlaybackService.ReplayStartAudioFeedbackPath : null,
                                    endIndicatorFileName: SettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? audioPlaybackService.ReplayStopAudioFeedbackPath : null);
                            }

                            actionService.TogglePlayPauseAudio();
                        }

                        TaskBar_ProgressState = "None";

                        if (SettingService.ShowBalloonTipsForRecording)
                        {
                            _notificationManager.ShowAsync(
                                content: new NotificationContent()
                                {
                                    Type = NotificationType.Success,
                                    Title = "Recording saved!",
                                    Message = AudioRecordingService.LastFileName
                                },
                                onClick: () =>
                                {
                                    if (AudioRecordingService.LastFileName == string.Empty || !File.Exists(AudioRecordingService.LastFileName))
                                    {
                                        return;
                                    }

                                    string argument = "/select, \"" + AudioRecordingService.LastFileName + "\"";
                                    System.Diagnostics.Process.Start("explorer.exe", argument);
                                });
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
                case nameof(AudioPlaybackService.Stopped):
                {
                    TaskBar_ProgressState = AudioPlaybackService.Stopped || AudioRecordingService.CurrentlyRecording ? "None" : "Normal";
                    break;
                }
            }
        }

        private void SettingService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SettingService.MinimizedToTray):
                {
                    if (SettingService.MinimizedToTray)
                    {
                        SwitchToBackgroundMode();
                    }
                    else
                    {
                        SwitchToForegroundMode();
                    }

                    break;
                }

                case nameof(SettingService.WindowAlwaysOnTop):
                {
                    this.Topmost = SettingService.WindowAlwaysOnTop;
                    break;
                }

                case nameof(SettingService.OutputPathControlVisible):
                {
                    OutputPathControl.Visibility = SettingService.OutputPathControlVisible ? Visibility.Visible : Visibility.Collapsed;
                    OutputPathControlSpacer.Visibility = SettingService.OutputPathControlVisible ? Visibility.Visible : Visibility.Collapsed;
                    break;
                }

                case nameof(SettingService.SelectedFileControlVisible):
                {
                    SelectedFileControl.Visibility = SettingService.SelectedFileControlVisible ? Visibility.Visible : Visibility.Collapsed;
                    SelectedFileControlSpacer.Visibility = SettingService.SelectedFileControlVisible ? Visibility.Visible : Visibility.Collapsed;
                    break;
                }

                case nameof(SettingService.RecordingTimerControlVisible):
                {
                    RecordingTimerControl.Visibility = SettingService.RecordingTimerControlVisible ? Visibility.Visible : Visibility.Collapsed;
                    RecordingTimerControlSpacer.Visibility = SettingService.RecordingTimerControlVisible ? Visibility.Visible : Visibility.Collapsed;
                    break;
                }
            }
        }

        private void WindowOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            actionService.ShowApplicationMenu();
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

        private void SwitchToBackgroundMode()
        {
            this.Hide();
            _notificationManager.ShowAsync(new NotificationContent() { Type = NotificationType.Information, Title = "Running in background now!", Message = @"RecNForget is now running in the background. Double click tray icon to restore" });
        }

        private void SwitchToForegroundMode()
        {
            this.Show();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MainWindow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            actionService.ShowApplicationMenu();
        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            if ((e as System.Windows.Forms.MouseEventArgs).Button == MouseButtons.Right)
            {
                actionService.ShowApplicationMenu();
            }
        }

        private void TrayIconMenu_DoubleClick(object sender, EventArgs e)
        {
            SettingService.MinimizedToTray = false;
            SwitchToForegroundMode();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // ToDo these keys should be configurable
            // ensure not triggering any of these if hotkey to record is the same

            var recHotkey = settingService.GetHotkeySettingAsList(SettingService.HotKey_StartStopRecording, string.Empty, string.Empty);

            if (!recHotkey.Contains(e.Key.ToString()))
            {
                if (e.Key == Key.Return)
                {
                    actionService.ChangeSelectedFileName();
                }
                else if (e.Key == Key.Delete)
                {
                    actionService.DeleteSelectedFile();
                }
                else if (e.Key == Key.Down)
                {
                    actionService.OpenOutputFolderInExplorer();
                }
                else if (e.Key == Key.Left)
                {
                    actionService.SelectPreviousFile();
                }
                else if (e.Key == Key.Right)
                {
                    actionService.SelectNextFile();
                }
                else if (e.Key == Key.Space)
                {
                    actionService.TogglePlayPauseSelectedFile();
                }
                else if (e.Key == Key.Escape)
                {
                    actionService.StopPlayingSelectedFile();
                }
                else if (e.Key == Key.X)
                {
                    actionService.ExportSelectedFile();
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            actionService.Exit();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}