using FMUtils.KeyboardHook;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Octokit;
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

namespace RecNForget
{
    /// <summary>
    /// Interaction logic for DownloadDialog.xaml
    /// </summary>
    public partial class DownloadDialog : Window, INotifyPropertyChanged
    {
        private bool cancelled;
        private bool succeeded;
        private bool installAfterDownload;
        private string targetDownloadPath;
        private ReleaseAsset asset;
        private WebClient client;

        public DownloadDialog(ReleaseAsset asset, bool installAfterDownload)
		{
            InitializeComponent();
            DataContext = this;

            this.cancelled = false;
            this.succeeded = false;
            this.installAfterDownload = installAfterDownload;
            this.targetDownloadPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), asset.Name);

            this.asset = asset;
            this.client = new WebClient();
            this.client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
            this.client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadProgressCompleted);

            DownloadProgress = "0";
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
                client.CancelAsync();
            }

            base.OnClosing(e);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            client.DownloadFileAsync(
                new Uri(asset.BrowserDownloadUrl), 
                targetDownloadPath);
        }

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
		}

		void DownloadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DownloadProgress = e.ProgressPercentage.ToString();
        }

        void DownloadProgressCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DownloadProgress = "Completed";
            CancelButton.IsEnabled = false;

            if (!cancelled)
            {
                succeeded = true;
                if (installAfterDownload)
                {
                    Process.Start(targetDownloadPath);
                }
                else
                {
                    Process.Start("explorer.exe", "/select, \"" + targetDownloadPath + "\"");
                }
            }

            this.Close();
        }

        private string downloadProgress;
        public string DownloadProgress
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
