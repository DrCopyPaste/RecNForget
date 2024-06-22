using Microsoft.Extensions.DependencyInjection;
using PressingIssue.Services.Contracts;
using PressingIssue.Services.Win32;
using RecNForget.Services;
using RecNForget.Services.Contracts;
using System;

namespace RecNForget.Controls.IoC;

public class ConfiguredServices
{
    public static ServiceProvider ServiceProvider
    {
        get
        {
            if (internalServiceProvider == null)
            {
                internalServiceProvider = ServiceCollection.BuildServiceProvider();
            }

            return internalServiceProvider;
        }
    }

    public static ServiceCollection ServiceCollection
    {
        get
        {
            if (internalServiceCollection == null)
            {
                internalServiceCollection = new ServiceCollection();
                AddConfiguredServices(internalServiceCollection);
            }

            return internalServiceCollection;
        }
    }

    private static ServiceCollection internalServiceCollection = null;
    private static ServiceProvider internalServiceProvider = null;
    private static void AddConfiguredServices(ServiceCollection serviceCollection)
    {
        //serviceCollection.AddSingleton<IActionService, ActionService>();

        serviceCollection.AddSingleton<IAppSettingService, AppSettingService>();
        serviceCollection.AddSingleton<ISelectedFileService, SelectedFileService>();
        serviceCollection.AddSingleton<IAudioPlaybackService, AudioPlaybackService>();
        serviceCollection.AddSingleton<IApplicationHotkeyService, ApplicationHotkeyService>();
        serviceCollection.AddSingleton<ISimpleGlobalHotkeyService, SimpleGlobalHotkeyService>();
        serviceCollection.AddSingleton<IAudioRecordingService, AudioRecordingService>();

        serviceCollection.AddSingleton<AboutWindow>();
        //serviceCollection.AddSingleton<DownloadDialog>();
        serviceCollection.AddSingleton<HelpWindow>();
        //serviceCollection.AddSingleton<HotkeyPromptWindow>();
        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddSingleton<NewToApplicationWindow>();
        //serviceCollection.AddSingleton<NewToVersionDialog>();
        //serviceCollection.AddSingleton<QuickTipDialog>();
        //serviceCollection.AddSingleton<ReleaseInstallationDialog>();
        serviceCollection.AddSingleton<SettingsWindow>();
    }
}
