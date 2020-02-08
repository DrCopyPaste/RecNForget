using FMUtils.KeyboardHook;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Octokit;
using RecNForget.Controls;
using RecNForget.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecNForget.Windows
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        private ApplicationBase appBase;

        public AboutDialog()
        {
            InitializeComponent();

            this.appBase = new ApplicationBase();
            VersionLabel.Text = string.Format("{0} - {1}", appBase.Info.Version.ToString(), appBase.Info.Title);
        }

        private void CheckForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { CheckForUpdates(); });
        }

        private async void CheckForUpdates()
        {
            try
            {
                var newerReleases = await UpdateChecker.GetNewerReleases(oldVersionString: appBase.Info.Version.ToString());

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
                this.DragMove();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
