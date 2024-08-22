using Microsoft.Extensions.DependencyInjection;
using PressingIssue.Services.Contracts;
using PressingIssue.Services.Win32;
using RecNForget.Services;
using RecNForget.Services.Contracts;
using RecNForget.ViewModels;
using System;
using System.Windows.Controls;

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
        serviceCollection.AddSingleton<IAppSettingService>(UserConfigurationService.Init());
        serviceCollection.AddSingleton<ISelectedFileService, SelectedFileService>();
        serviceCollection.AddSingleton<IAudioPlaybackService, AudioPlaybackService>();
        serviceCollection.AddSingleton<IApplicationHotkeyService, ApplicationHotkeyService>();
        serviceCollection.AddSingleton<ISimpleGlobalHotkeyService, SimpleGlobalHotkeyService>();
        serviceCollection.AddSingleton<IAudioRecordingService, AudioRecordingService>();

        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddTransient<AboutWindow>();
        //serviceCollection.AddSingleton<DownloadDialog>();
        serviceCollection.AddTransient<HelpWindow>();
        //serviceCollection.AddSingleton<HotkeyPromptWindow>();
        
        serviceCollection.AddTransient<NewToApplicationWindow>();
        serviceCollection.AddSingleton<NewToVersionDialog>();
        serviceCollection.AddTransient<QuickTipDialog>();
        //serviceCollection.AddSingleton<ReleaseInstallationDialog>();
        serviceCollection.AddTransient<SettingsWindow>();

        serviceCollection.AddSingleton<AboutViewModel>();
        serviceCollection.AddSingleton<MainViewModel>();
        serviceCollection.AddSingleton<NewToApplicationViewModel>();
        serviceCollection.AddSingleton<NewToVersionViewModel>();
        serviceCollection.AddSingleton<QuickTipViewModel>();
        serviceCollection.AddSingleton<SettingsViewModel>();

        serviceCollection.AddSingleton<ContextMenuCommands>();
    }
}
