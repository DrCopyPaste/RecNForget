using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Notifications.Wpf.Core;
using RecNForget.Controls.Services;
using RecNForget.Controls;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using RecNForget.Controls.Extensions;

namespace RecNForget.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    private readonly IAppSettingService appSettingService;
    private readonly NotificationManager notificationManager = new NotificationManager();

    public AboutViewModel(IAppSettingService appSettingService)
    {
        this.appSettingService = appSettingService;

        var assemblyInformationalVersion = appSettingService.RuntimeInformalVersionString;
        var assemblyFileVersion = new Version(appSettingService.RuntimeVersionString);

        AppNameAndVersion = string.Format("RecNForget {0}", string.Format("{0}.{1}.{2}", assemblyFileVersion.Major, assemblyFileVersion.Minor, assemblyFileVersion.Build));
        VersionLabel = string.Format("{0} - v{1}", "Chili Garlic Shrimps", assemblyInformationalVersion);
    }

    [RelayCommand]
    private async Task<bool> CheckForUpdates(bool showMessages = true)
    {
        if (checkingForUpdates) return false;
        checkingForUpdates = true;

        try
        {
            var newerReleases = await UpdateChecker.GetNewerReleases(oldVersionString: appSettingService.RuntimeVersionString);

            if (newerReleases.Any())
            {
                string changeLog = UpdateChecker.GetAllChangeLogs(newerReleases);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var installUpdateDialog = new ReleaseInstallationDialog(newerReleases.First(), UpdateChecker.GetValidVersionStringMsiAsset(newerReleases.First()), changeLog);

                    // installUpdateDialog.TrySetViewablePositionFromOwner(OwnerControl);
                    installUpdateDialog.ShowDialog();
                });
            }
            else
            {
                if (showMessages)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        notificationManager.ShowAsync(
                            content: new NotificationContent()
                            {
                                Title = "No newer version found.",
                                Message = "RecNForget is already up to date.",
                                Type = NotificationType.Information
                            },
                            expirationTime: TimeSpan.FromSeconds(10));
                    });
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            if (showMessages)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    notificationManager.ShowAsync(
                        content: new NotificationContent()
                        {
                            Title = "Error during update",
                            Message = "An error occurred trying to get updates:",
                            Type = NotificationType.Error
                        },
                        expirationTime: TimeSpan.FromSeconds(10));
                });
            }
        }
        finally
        {
            checkingForUpdates = false;
        }

        return false;
    }

    [ObservableProperty]
    private string appNameAndVersion;

    [ObservableProperty]
    private string versionLabel;
    private bool checkingForUpdates;
}
