using Microsoft.Extensions.DependencyInjection;
using Notifications.Wpf.Core;
using RecNForget.Controls.IoC;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.ViewModels;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private IAudioRecordingService audioRecordingService = null;
        private IApplicationHotkeyService hotkeyService = null;
        private IAppSettingService settingService = null;
        private IAudioPlaybackService audioPlaybackService = null;
        private ISelectedFileService selectedFileService = null;

        private readonly NotificationManager _notificationManager = new NotificationManager();

        public MainWindow(
            MainViewModel mainViewModel,
            IAudioRecordingService audioRecordingService,
            IApplicationHotkeyService hotkeyService,
            IAppSettingService settingService,
            IAudioPlaybackService audioPlaybackService,
            ISelectedFileService selectedFileService)
        {
            DataContext = mainViewModel;
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.hotkeyService = new DesignerApplicationHotkeyService();
                SelectedFileService = new DesignerSelectedFileService();
                SettingService = new DesignerAppSettingService();
                AudioRecordingService = new DesignerAudioRecordingService();
                AudioPlaybackService = new DesignerAudioPlaybackService();
                return;
            }
            else
            {
                this.hotkeyService = hotkeyService;
                SelectedFileService = selectedFileService;
                SettingService = settingService;

                AudioRecordingService = audioRecordingService;

                AudioPlaybackService = audioPlaybackService;

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

                //this.KeyDown += Window_KeyDown;
                this.MouseRightButtonUp += MainWindow_MouseRightButtonUp;

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
                //else
                //{
                //    SwitchToForegroundMode();
                //}
            }
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

        protected override void OnClosing(CancelEventArgs e)
        {
            SettingService.MainWindowLeftX = this.Left;
            SettingService.MainWindowTopY = this.Top;
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
            var menuCommands = ConfiguredServices.ServiceProvider.GetRequiredService<ContextMenuCommands>();
            menuCommands.ContextMenu.IsOpen = true;
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
            var menuCommands = ConfiguredServices.ServiceProvider.GetRequiredService<ContextMenuCommands>();
            menuCommands.ContextMenu.IsOpen = true;
        }

        //private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    // ToDo these keys should be configurable
        //    // ensure not triggering any of these if hotkey to record is the same

        //    var recHotkey = settingService.GetHotkeySettingAsList(SettingService.HotKey_StartStopRecording, string.Empty, string.Empty);

        //    if (!recHotkey.Contains(e.Key.ToString()))
        //    {
        //        if (e.Key == Key.Return)
        //        {
        //            actionService.ChangeSelectedFileName();
        //        }
        //        else if (e.Key == Key.Delete)
        //        {
        //            actionService.DeleteSelectedFile();
        //        }
        //        else if (e.Key == Key.Down)
        //        {
        //            actionService.OpenOutputFolderInExplorer();
        //        }
        //        else if (e.Key == Key.Left)
        //        {
        //            actionService.SelectPreviousFile();
        //        }
        //        else if (e.Key == Key.Right)
        //        {
        //            actionService.SelectNextFile();
        //        }
        //        else if (e.Key == Key.Space)
        //        {
        //            actionService.TogglePlayPauseSelectedFile();
        //        }
        //        else if (e.Key == Key.Escape)
        //        {
        //            actionService.StopPlayingSelectedFile();
        //        }
        //        else if (e.Key == Key.X)
        //        {
        //            actionService.ExportSelectedFile();
        //        }
        //    }
        //}

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}