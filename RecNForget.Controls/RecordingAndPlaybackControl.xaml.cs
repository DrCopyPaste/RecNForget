using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RecNForget.Controls.Services;
using RecNForget.IoC;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using Unity;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for RecordingAndPlaybackControl.xaml
    /// </summary>
    public partial class RecordingAndPlaybackControl : UserControl, INotifyPropertyChanged
    {
        private readonly IActionService actionService = null;
        private readonly IAppSettingService appSettingService = null;
        private IAudioPlaybackService audioPlaybackService = null;
        private IAudioRecordingService audioRecordingService;
        private ISelectedFileService selectedFileService = null;

        public RecordingAndPlaybackControl()
        {
            DataContext = this;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.actionService = new DesignerActionService();
                this.appSettingService = new DesignerAppSettingService();

                AudioPlaybackService = new DesignerAudioPlaybackService();
                AudioRecordingService = new DesignerAudioRecordingService();
                SelectedFileService = new DesignerSelectedFileService();
            }
            else
            {
                this.actionService = new ActionService();
                this.appSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
                AudioPlaybackService = UnityHandler.UnityContainer.Resolve<IAudioPlaybackService>();
                AudioRecordingService = UnityHandler.UnityContainer.Resolve<IAudioRecordingService>();
                SelectedFileService = UnityHandler.UnityContainer.Resolve<ISelectedFileService>();

                AudioPlaybackService.PropertyChanged += AudioPlaybackService_PropertyChanged;
                AudioRecordingService.PropertyChanged += AudioRecordingService_PropertyChanged;
            }

            InitializeComponent();
        }

        ~RecordingAndPlaybackControl()
        {
            AudioPlaybackService.PropertyChanged -= AudioPlaybackService_PropertyChanged;
            AudioRecordingService.PropertyChanged -= AudioRecordingService_PropertyChanged;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AudioRecordingService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AudioRecordingService.CurrentlyRecording):
                {
                    if (AudioRecordingService.CurrentlyRecording)
                    {
                        RecordButton.Style = (Style)FindResource("StopRecordButton");
                    }
                    else
                    {
                        RecordButton.Style = (Style)FindResource("RecordButton");
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
                    TogglePlaySelectedFileButton.Style = AudioPlaybackService.Playing && AudioRecordingService.CurrentlyNotRecording ? (Style)FindResource("PauseButton") : (Style)FindResource("PlayButton");
                    break;
                }
            }
        }

        private void OpenOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            actionService.OpenOutputFolderInExplorer();
        }

        private void StopReplayLastRecording_Click(object sender, RoutedEventArgs e)
        {
            actionService.StopPlayingSelectedFile();
        }

        private void SkipPrevButton_Click(object sender, RoutedEventArgs e)
        {
            actionService.SelectPreviousFile();
        }

        private void SkipNextButton_Click(object sender, RoutedEventArgs e)
        {
            actionService.SelectNextFile();
        }

        private void ReplayLastRecording_Click(object sender, RoutedEventArgs e)
        {
            actionService.TogglePlayPauseSelectedFile();
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            actionService.ToggleStartStopRecording();
        }
    }
}
