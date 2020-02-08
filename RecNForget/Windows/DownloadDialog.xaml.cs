using FMUtils.KeyboardHook;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Octokit;
using RecNForget.Controls;
using RecNForget.Services;
using RecNForget.Services.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
    /// Interaction logic for DownloadDialog.xaml
    /// </summary>
    public partial class DownloadDialog : Window, INotifyPropertyChanged
    {
        private bool cancelled;
        private string targetDownloadPath;
        private ReleaseAsset asset;
        private HttpClient httpClient;

        public DownloadDialog(ReleaseAsset asset, string targetDownloadPath)
        {
            InitializeComponent();
            DataContext = this;

            this.cancelled = false;
            this.succeeded = false;
            this.targetDownloadPath = targetDownloadPath;
            this.asset = asset;
            this.httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromMilliseconds(60000)
            };

            DownloadProgress = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!succeeded)
            {
                cancelled = true;
            }

            base.OnClosing(e);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);


            var task = Task.Run(() => { DownloadAsset(asset); });
        }

        private async void DownloadAsset(ReleaseAsset asset)
        {
            try
            {
                Progress<float> downloadProgress = new Progress<float>();
                downloadProgress.ProgressChanged += (s, e) =>
                {
                    DownloadProgress = e * 100;
                };

                using (var fileStream = new FileStream(targetDownloadPath, System.IO.FileMode.Create, FileAccess.Write))
                {
                    await httpClient.DownloadAsync(asset.BrowserDownloadUrl, fileStream, downloadProgress);
                }

                succeeded = true;
            }
            catch (Exception e)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => {
                    var errorMessageBox = new CustomMessageBox(
                    caption: "An error occurred",
                    buttons: CustomMessageBoxButtons.OK,
                    icon: CustomMessageBoxIcon.Error,
                    messageRows: new List<string>()
                    {
                        string.Format("An error occurred trying to download {0} to {1}", asset.BrowserDownloadUrl, targetDownloadPath),
                        e.Message
                    },
                    controlFocus: CustomMessageBoxFocus.Ok);

                    errorMessageBox.ShowDialog();
                });
            }
            finally
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => {
                    this.Close();
                });
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private bool succeeded;
        public bool Succeeded
        {
            get
            {
                return succeeded;
            }
        }

        private float downloadProgress;
        public float DownloadProgress
        {
            get
            {
                return downloadProgress;
            }
            set
            {
                downloadProgress = value;
                OnPropertyChanged();
            }
        }
    }
}
