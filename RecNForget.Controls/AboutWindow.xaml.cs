using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using RecNForget.Controls.IoC;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.WPF.Services.Contracts;
using Unity;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        private readonly IAppSettingService appSettingService;
        private readonly IActionService actionService;

        public AboutWindow()
        {
            DataContext = this;
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.appSettingService = new DesignerAppSettingService();
                this.actionService = new DesignerActionService();
                return;
            }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;

                this.appSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
                this.actionService = UnityHandler.UnityContainer.Resolve<IActionService>();
            }

            var assemblyInformationalVersion = appSettingService.RuntimeInformalVersionString;
            var assemblyFileVersion = new Version(appSettingService.RuntimeVersionString);

            AppNameAndVersion.Text = string.Format("RecNForget {0}", string.Format("{0}.{1}.{2}", assemblyFileVersion.Major, assemblyFileVersion.Minor, assemblyFileVersion.Build));
            VersionLabel.Text = string.Format("{0} - v{1}", "Chili Garlic Shrimps", assemblyInformationalVersion);
        }

        private void CheckForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { actionService.CheckForUpdates(true); });
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {e.Uri.AbsoluteUri}") { CreateNoWindow = true });
            e.Handled = true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
