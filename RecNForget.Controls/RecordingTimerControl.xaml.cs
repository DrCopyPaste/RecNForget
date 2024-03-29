﻿using RecNForget.Controls.IoC;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.WPF.Services.Contracts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for RecordingTimerControl.xaml
    /// </summary>
    public partial class RecordingTimerControl : UserControl, INotifyPropertyChanged
    {
        private IAppSettingService settingService;
        private IActionService actionService;
        private IAudioRecordingService audioRecordingService;

        public RecordingTimerControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                ActionService = new DesignerActionService();
                SettingService = new DesignerAppSettingService();
                AudioRecordingService = new DesignerAudioRecordingService();

                return;
            }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;

                ActionService = UnityHandler.UnityContainer.Resolve<IActionService>();
                SettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
                AudioRecordingService = UnityHandler.UnityContainer.Resolve<IAudioRecordingService>();
            }
        }
        public IActionService ActionService
        {
            get
            {
                return actionService;
            }

            set
            {
                actionService = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StopAfter_Checked_Changed(object sender, RoutedEventArgs e)
        {
            if (StopAfter_CheckBox.IsChecked.HasValue && StopAfter_CheckBox.IsChecked.Value)
            {
                if (AudioRecordingService.CurrentlyRecording)
                {
                    AudioRecordingService.StartTimerToStopRecordingAfter();
                }
            }

            if (!AudioRecordingService.TimerForRecordingStopAfterNotRunning && (!StopAfter_CheckBox.IsChecked.HasValue || !StopAfter_CheckBox.IsChecked.Value))
            {
                AudioRecordingService.ResetStopAfterDispatcherTimer();
            }
        }

        private void StartAfter_Checked_Changed(object sender, RoutedEventArgs e)
        {
            if (!AudioRecordingService.TimerForRecordingStartAfterNotRunning && (!StartAfter_CheckBox.IsChecked.HasValue || !StartAfter_CheckBox.IsChecked.Value))
            {
                AudioRecordingService.ResetStartAfterDispatcherTimer();
            }
        }
    }
}
