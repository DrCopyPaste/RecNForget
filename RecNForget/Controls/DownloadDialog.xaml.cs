using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Notifications.Wpf.Core;
using Octokit;
using RecNForget.Controls;
using RecNForget.Controls.Extensions;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for DownloadDialog.xaml
    /// </summary>
    public partial class DownloadDialog : Window, INotifyPropertyChanged
    {
        private readonly NotificationManager _notificationManager = new NotificationManager();
        private string targetDownloadPath;
        private ReleaseAsset asset;
        private HttpClient httpClient;
        private float downloadProgress;

        public DownloadDialog(ReleaseAsset asset, string targetDownloadPath)
        {
            DataContext = this;
            InitializeComponent();

            this.Succeeded = false;
            this.targetDownloadPath = targetDownloadPath;
            this.asset = asset;
            this.httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromMilliseconds(60000)
            };

            DownloadProgress = 0;

            if (DesignerProperties.GetIsInDesignMode(this))
            { }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Succeeded { get; private set; }

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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            var task = Task.Run(() => { DownloadAsset(asset); });
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

                Succeeded = true;
            }
            catch (Exception e)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    _notificationManager.ShowAsync(
                        content: new NotificationContent()
                        {
                            Title = "Error trying to download update",
                            Message = string.Format("An error occurred trying to download {0} to {1}. \n{2}", asset.BrowserDownloadUrl, targetDownloadPath, e.Message),
                            Type = NotificationType.Error
                        },
                        expirationTime: TimeSpan.FromSeconds(10));
                });
            }
            finally
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    this.Close();
                });
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
