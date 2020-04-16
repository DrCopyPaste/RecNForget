using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RecNForget.Control.Services;
using RecNForget.IoC;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using Unity;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for SelectedFileControl.xaml
    /// </summary>
    public partial class SelectedFileControl : UserControl, INotifyPropertyChanged
    {
        private readonly IActionService actionService = null;
        private readonly IAppSettingService appSettingService = null;
        private readonly IAudioPlaybackService audioPlaybackService = null;

        private ISelectedFileService selectedFileService = null;

        public SelectedFileControl()
        {
            DataContext = this;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.actionService = new DesignerActionService();
                this.appSettingService = new DesignerAppSettingService();
                this.audioPlaybackService = new DesignerAudioPlaybackService();

                this.SelectedFileService = new DesignerSelectedFileService();
            }
            else
            {
                this.actionService = new ActionService();
                this.appSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
                this.audioPlaybackService = UnityHandler.UnityContainer.Resolve<IAudioPlaybackService>();

                this.SelectedFileService = UnityHandler.UnityContainer.Resolve<ISelectedFileService>();
            }

            InitializeComponent();
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChangeSelectedFileNameButton_Clicked(object sender, RoutedEventArgs e)
        {
            actionService.ChangeSelectedFileName(this);
        }

        private void DeleteSelectedFileButton_Clicked(object sender, RoutedEventArgs e)
        {
            actionService.DeleteSelectedFile(this);
        }
    }
}
