using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RecNForget.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    private readonly IAppSettingService appSettingService;
    private readonly IActionService actionService;

    public AboutViewModel(IAppSettingService appSettingService, IActionService actionService)
    {
        this.actionService = actionService;
        this.appSettingService = appSettingService;

        var assemblyInformationalVersion = appSettingService.RuntimeInformalVersionString;
        var assemblyFileVersion = new Version(appSettingService.RuntimeVersionString);

        AppNameAndVersion = string.Format("RecNForget {0}", string.Format("{0}.{1}.{2}", assemblyFileVersion.Major, assemblyFileVersion.Minor, assemblyFileVersion.Build));
        VersionLabel = string.Format("{0} - v{1}", "Chili Garlic Shrimps", assemblyInformationalVersion);
    }

    [RelayCommand]
    private async Task<bool> CheckForUpdates()
    {
        var result = await actionService.CheckForUpdatesAsync(true);
        return result;
    }

    [ObservableProperty]
    private string appNameAndVersion;

    [ObservableProperty]
    private string versionLabel;
}
