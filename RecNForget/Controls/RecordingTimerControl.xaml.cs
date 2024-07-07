using Microsoft.Extensions.DependencyInjection;
using RecNForget.Controls.IoC;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for RecordingTimerControl.xaml
    /// </summary>
    public partial class RecordingTimerControl : UserControl, INotifyPropertyChanged
    {
        private IAppSettingService settingService;
        private IAudioRecordingService audioRecordingService;

        public RecordingTimerControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                SettingService = new DesignerAppSettingService();
                AudioRecordingService = new DesignerAudioRecordingService();

                return;
            }
            else
            {
                SettingService = ConfiguredServices.ServiceProvider.GetRequiredService<IAppSettingService>();
                AudioRecordingService = ConfiguredServices.ServiceProvider.GetRequiredService<IAudioRecordingService>();
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
