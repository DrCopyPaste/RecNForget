using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.VisualBasic.ApplicationServices;
using RecNForget.Controls;
using RecNForget.Controls.Services;
using RecNForget.Services;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;

namespace RecNForget.Windows
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        private ApplicationBase appBase;
        private readonly IAppSettingService appSettingService;
        private readonly IActionService actionService;

        public AboutDialog(IAppSettingService appSettingService)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.appSettingService = new DesignerAppSettingService();
                this.actionService = new DesignerActionService();
            }
            else
            {
                this.appSettingService = appSettingService;
                this.actionService = new ActionService();
            }

            InitializeComponent();

            var assemblyInformationalVersion = ThisAssembly.AssemblyInformationalVersion;
            var assemblyFileVersion = new Version(ThisAssembly.AssemblyFileVersion);
            this.appBase = new ApplicationBase();

            AppNameAndVersion.Text = string.Format("RecNForget {0}", string.Format("{0}.{1}.{2}", assemblyFileVersion.Major, assemblyFileVersion.Minor, assemblyFileVersion.Build));
            VersionLabel.Text = string.Format("{0} - v{1}", appBase.Info.Title, assemblyInformationalVersion);
        }

        private void CheckForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { actionService.CheckForUpdates(this, true); });
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
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
