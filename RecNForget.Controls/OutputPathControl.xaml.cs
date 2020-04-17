using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Ookii.Dialogs.Wpf;
using RecNForget.Controls.Services;
using RecNForget.IoC;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using Unity;

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

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.actionService = new DesignerActionService();
                this.appSettingService = new DesignerAppSettingService();
                this.audioRecordingService = new DesignerAudioRecordingService();
                this.selectedFileService = new DesignerSelectedFileService();

            }
            else
            {
                this.actionService = new ActionService();
                this.appSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
                this.audioRecordingService = UnityHandler.UnityContainer.Resolve<IAudioRecordingService>();

                SelectedFileService = UnityHandler.UnityContainer.Resolve<ISelectedFileService>();
            }

            appSettingService.PropertyChanged += AppSettingService_PropertyChanged;
            UpdateOutputPathWithFileNamePattern();

            InitializeComponent();
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

        private void FilenamePrefix_Changed(object sender, RoutedEventArgs e)
        {
            // https://stackoverflow.com/a/23182807
            //appSettingService.FilenamePrefix = string.Concat(appSettingService.FilenamePrefix.Split(Path.GetInvalidFileNameChars()));
        }

        private void ChangeFileNamePatternButton_Clicked(object sender, RoutedEventArgs e)
        {
            CustomMessageBox tempDialog = new CustomMessageBox(
                caption: "Type in a new pattern for file name generation.",
                icon: CustomMessageBoxIcon.Question,
                buttons: CustomMessageBoxButtons.OkAndCancel,
                messageRows: new List<string>() { "Supported placeholders:", "(Date), (Guid)", "If you do not provide a placeholder to create unique file names, RecNForget will do it for you." },
                prompt: appSettingService.FilenamePrefix,
                controlFocus: CustomMessageBoxFocus.Prompt,
                promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

            if (!appSettingService.MinimizedToTray)
            {
                tempDialog.Owner = Window.GetWindow(this);
            }

            if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
            {
                appSettingService.FilenamePrefix = tempDialog.PromptContent;
            }
        }

        private void ChangeOutputFolderButton_Clicked(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();

            if (dialog.ShowDialog(Window.GetWindow(this)) == true)
            {
                appSettingService.OutputPath = dialog.SelectedPath;
                SelectedFileService.SelectLatestFile();
            }
        }
    }
}
