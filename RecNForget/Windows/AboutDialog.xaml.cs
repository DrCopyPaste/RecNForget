using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.VisualBasic.ApplicationServices;
using RecNForget.Controls;
using RecNForget.Services;
using RecNForget.Services.Contracts;

namespace RecNForget.Windows
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        private ApplicationBase appBase;
        private readonly IAppSettingService appSettingService;

        public AboutDialog(IAppSettingService appSettingService)
        {
            this.appSettingService = appSettingService;

            InitializeComponent();

            var assemblyInformationalVersion = ThisAssembly.AssemblyInformationalVersion;
            var assemblyFileVersion = new Version(ThisAssembly.AssemblyFileVersion);
            this.appBase = new ApplicationBase();

            AppNameAndVersion.Text = string.Format("RecNForget {0}", string.Format("{0}.{1}.{2}", assemblyFileVersion.Major, assemblyFileVersion.Minor, assemblyFileVersion.Build));
            VersionLabel.Text = string.Format("{0} - v{1}", appBase.Info.Title, assemblyInformationalVersion);
        }

        private void CheckForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { CheckForUpdates(); });
        }

        private async void CheckForUpdates()
        {
            try
            {
                var newerReleases = await UpdateChecker.GetNewerReleases(oldVersionString: ThisAssembly.AssemblyFileVersion);

                if (newerReleases.Any())
                {
                    string changeLog = UpdateChecker.GetAllChangeLogs(newerReleases);

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        var installUpdateDialog = new ReleaseInstallationDialog(newerReleases.First(), UpdateChecker.GetValidVersionStringMsiAsset(newerReleases.First()), changeLog);
                        installUpdateDialog.ShowDialog();
                    });
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        CustomMessageBox tempDialog = new CustomMessageBox(
                            caption: "RecNForget is already up to date!",
                            icon: CustomMessageBoxIcon.Information,
                            buttons: CustomMessageBoxButtons.OK,
                            messageRows: new List<string>() { "No newer version found" },
                            controlFocus: CustomMessageBoxFocus.Ok);

                        tempDialog.Owner = Window.GetWindow(this);

                        tempDialog.ShowDialog();
                    });
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var errorDialog = new CustomMessageBox(
                        caption: "Error during update",
                        icon: CustomMessageBoxIcon.Error,
                        buttons: CustomMessageBoxButtons.OK,
                        messageRows: new List<string>() { "An error occurred trying to get updates:", ex.InnerException.Message },
                        controlFocus: CustomMessageBoxFocus.Ok);

                    errorDialog.ShowDialog();
                });
            }
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
