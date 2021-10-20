using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Unity;
using RecNForget.Controls.IoC;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.WPF.Services.Contracts;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for OutputPathControl.xaml
    /// </summary>
    public partial class OutputPathControl : UserControl, INotifyPropertyChanged
    {
        private readonly IActionService actionService = null;
        private readonly IAppSettingService appSettingService = null;
        private readonly IAudioRecordingService audioRecordingService = null;

        private ISelectedFileService selectedFileService = null;
        private string outputPathWithFilePattern;

        public OutputPathControl()
        {
            DataContext = this;
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.actionService = new DesignerActionService();
                this.appSettingService = new DesignerAppSettingService();
                this.audioRecordingService = new DesignerAudioRecordingService();
                this.selectedFileService = new DesignerSelectedFileService();

                OutputPathWithFilePattern = audioRecordingService.GetTargetPathTemplateString();
                return;
            }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;

                this.actionService = UnityHandler.UnityContainer.Resolve<IActionService>();
                this.appSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
                this.audioRecordingService = UnityHandler.UnityContainer.Resolve<IAudioRecordingService>();

                SelectedFileService = UnityHandler.UnityContainer.Resolve<ISelectedFileService>();

                appSettingService.PropertyChanged += AppSettingService_PropertyChanged;

                UpdateOutputPathWithFileNamePattern();
            }
        }

        ~OutputPathControl()
        {
            appSettingService.PropertyChanged -= AppSettingService_PropertyChanged;
        }

        private void AppSettingService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(appSettingService.OutputPath) || e.PropertyName == nameof(appSettingService.FilenamePrefix))
            {
                UpdateOutputPathWithFileNamePattern();
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

        public string OutputPathWithFilePattern
        {
            get
            {
                return outputPathWithFilePattern;
            }

            set
            {
                outputPathWithFilePattern = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateOutputPathWithFileNamePattern()
        {
            OutputPathWithFilePattern = audioRecordingService.GetTargetPathTemplateString();
        }

        private void ChangeFileNamePatternButton_Clicked(object sender, RoutedEventArgs e)
        {
            actionService.ChangeFileNamePattern();
        }

        private void ChangeOutputFolderButton_Clicked(object sender, RoutedEventArgs e)
        {
            actionService.ChangeOutputFolder();
        }
    }
}
