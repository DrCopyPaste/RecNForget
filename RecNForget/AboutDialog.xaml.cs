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
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
	{
        private ApplicationBase appBase;

		public AboutDialog()
		{
            InitializeComponent();

            this.appBase = new ApplicationBase();
            VersionLabel.Text = string.Format("Version: {0}", appBase.Info.Version.ToString());
        }

        private void CheckForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var newerReleases = GetNewerReleases(appBase.Info.Version.ToString());

            if (newerReleases.Any())
            {
                StringBuilder changeLog = new StringBuilder();

                // neweer releases found, aggregate all change logs and prepare download (and show in explorer) button as well as download and install button
                foreach(var release in newerReleases)
                {
                    changeLog.AppendLine("==========================================================================");
                    changeLog.AppendLine(string.Format("{0} - @ {1}", release.Name, release.HtmlUrl));
                    changeLog.AppendLine("==========================================================================");
                    changeLog.Append(release.Body);
                    changeLog.AppendLine();
                }

                var installUpdateDialog = new ReleaseInstallationDialog(newerReleases.First(), GetValidVersionStringMsiAsset(newerReleases.First()), changeLog.ToString());

                this.Close();
                installUpdateDialog.ShowDialog();
            }
            else
            {
                // show message box there is no newer release available
                System.Windows.MessageBox.Show("RecNForget is up to date!", "No newer version found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        // gets an ordered list, where the newest available release is the first element
        private IReadOnlyList<Release> GetNewerReleases(string oldVersionString)
        {
            var githubApiClient = new GitHubClient(new ProductHeaderValue("RecNForget"));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var allReleases = githubApiClient.Repository.Release.GetAll("DrCopyPaste", "RecNForget").GetAwaiter().GetResult();

            // filter all releases for only those that are not in draft mode or prerelease and contain a msi-setup file that satisfies a specific file pattern
            // and also are newer (according to Version-class parsing logic A.B.C.D
            // here we could check for possible breaking changes and only serve those releases with the same major release version, but I this project is so small that I do not see the necessity
            var allRealReleases = allReleases.Where(r =>
                !r.Draft
                && !r.Prerelease
                && r.Assets.Any(a => GetValidVersionStringMsiAsset(r) != null)
                && VersionIsNewer(oldVersionString, VersionStringFromMsiAsset(r.Assets.First(a => GetValidVersionStringMsiAsset(r) != null))))
                .OrderByDescending(r => new Version(VersionStringFromMsiAsset(r.Assets.First(a => GetValidVersionStringMsiAsset(r) != null))))
                .ToList();

            return allRealReleases;
        }

        // returns first msi file that satisfies the desired file pattern
        // otherwise returns null
        private ReleaseAsset GetValidVersionStringMsiAsset(Release release)
        {
            foreach (var asset in release.Assets.Where(a => a.Name.EndsWith(".msi")))
            {
                string versionString = asset.Name.Replace("RecNForget.Setup.", string.Empty).Replace(".msi", string.Empty);
                Version version;

                if (Version.TryParse(versionString, out version))
                {
                    return asset;
                }
            }

            return null;
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

        private bool VersionIsNewer(string currentVersionString, string possiblyNewerVersionString)
        {
            Version currentVersion = new Version(currentVersionString);
            Version possiblyNewerVersion = new Version(possiblyNewerVersionString);

            // CompareTo returns > 0 if left hand version is greater than right hand version
            // CompareTo returns = 0 if left hand version equal to right hand version
            // CompareTo returns < 0 if left hand version is smaller than right hand version
            return possiblyNewerVersion.CompareTo(currentVersion) > 0;
        }
    }
}
