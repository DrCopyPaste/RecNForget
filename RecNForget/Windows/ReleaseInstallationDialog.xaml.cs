using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Octokit;

namespace RecNForget.Windows
{
    /// <summary>
    /// Interaction logic for ReleaseInstallationDialog.xaml
    /// </summary>
    public partial class ReleaseInstallationDialog : Window, INotifyPropertyChanged
    {
        private ReleaseAsset asset;
        private string releaseNotes;
        private string versionString;
        private bool installAfterDownload;

        public ReleaseInstallationDialog(Release release, ReleaseAsset asset, string releaseNotes)
        {
            InitializeComponent();
            DataContext = this;

            this.KeyDown += Window_KeyDown;

            this.asset = asset;
            ReleaseNotes = releaseNotes;
            VersionInfoString = string.Format("{0} - {1} ({2} MB)", VersionStringFromMsiAsset(asset), release.Name, string.Format("{0:0.000}", asset.Size / 1024d / 1024d));
            InstallAfterDownload = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;    

        public string ReleaseNotes
        {
            get
            {
                return releaseNotes;
            }

            set
            {
                releaseNotes = value;
                OnPropertyChanged();
            }
        }

        public string VersionInfoString
        {
            get
            {
                return versionString;
            }

            set
            {
                versionString = value;
                OnPropertyChanged();
            }
        }

        public bool InstallAfterDownload
        {
            get
            {
                return installAfterDownload;
            }

            set
            {
                installAfterDownload = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var targetPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), asset.Name);

            var downloadDialog = new DownloadDialog(asset, targetPath)
            {
                Owner = System.Windows.Application.Current.MainWindow
            };

            downloadDialog.ShowDialog();

            if (downloadDialog.Succeeded)
            {
                if (InstallAfterDownload)
                {
                    Process.Start(targetPath);
                }
                else
                {
                    Process.Start("explorer.exe", "/select, \"" + targetPath + "\"");
                }
            }

            this.Close();
        }

        private string VersionStringFromMsiAsset(ReleaseAsset asset)
        {
            if (asset.Name.EndsWith(".msi"))
            {
                string versionString = asset.Name.Replace("RecNForget.Setup.", string.Empty).Replace(".msi", string.Empty);
                Version version;

                if (Version.TryParse(versionString, out version))
                {
                    return version.ToString();
                }
            }

            return string.Empty;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
