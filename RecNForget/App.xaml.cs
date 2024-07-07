using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
using RecNForget.Controls;
using RecNForget.Controls.Helper;
using RecNForget.Controls.IoC;
using RecNForget.Controls.Services;
using RecNForget.Services.Contracts;
using RecNForget.ViewModels;
using RecNForget.WPF.Services.Contracts;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RecNForget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow mainWindow = null;
        private IActionService actionService;

        [STAThread]
        public static void Main()
        {
            using (var appLock = new SingleInstanceApplicationLock())
            {
                if (!appLock.TryAcquireExclusiveLock())
                {
                    // we dont have styles loaded at this point so we just show custom windwows messagebox
                    MessageBox.Show("Another instance of RecNForget is already running, closing this one...", "RecNForget is already running.", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
        }

        public static ContextMenu ContextMenu { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            ConfiguredServices.ServiceCollection.AddSingleton<IActionService, ActionService>();
            ConfiguredServices.ServiceCollection.BuildServiceProvider();

            var appSettingService = ConfiguredServices.ServiceProvider.GetRequiredService<IAppSettingService>();

            if (e.Args.Length > 0 && !string.IsNullOrEmpty(e.Args[0]) && e.Args[0] == "-removeAppData")
            {
                try
                {
                    appSettingService.AutoStartWithWindows = false;
                    appSettingService.RemoveAppConfigSettingFile();

                    Environment.Exit(0);
                }
                catch
                {
                    Environment.Exit(1);
                }
            }

            base.OnStartup(e);

            var hotkeyService = ConfiguredServices.ServiceProvider.GetRequiredService<IApplicationHotkeyService>();

            actionService = ConfiguredServices.ServiceProvider.GetRequiredService<IActionService>();
            ThemeManager.ChangeTheme(appSettingService.WindowTheme);

            // Show main window first, so that windows popping up (like new updates/new to app) are in foreground and escapable
            mainWindow = ConfiguredServices.ServiceProvider.GetRequiredService<MainWindow>();
            actionService.OwnerControl = mainWindow;

            HandleFirstStartAndUpdates(actionService, appSettingService, hotkeyService);

            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/RecNForget;component/Images/logo.ico")).Stream;
            var icon = new System.Drawing.Icon(iconStream);
            var menuCommands = ConfiguredServices.ServiceProvider.GetRequiredService<ContextMenuCommands>();

            ContextMenu = menuCommands.ContextMenu;

            TaskbarIcon taskbarIcon = new TaskbarIcon
            {
                Icon = icon,
                ContextMenu = ContextMenu
            };

            taskbarIcon.ForceCreate();
        }

        private void HandleFirstStartAndUpdates(IActionService actionService, IAppSettingService appSettingService, IApplicationHotkeyService hotkeyService)
        {
            var previouslyInstalledVersion = appSettingService.LastInstalledVersion;
            hotkeyService.ResetAndReadHotkeysFromConfig();

            var configVersionWasUpdated = appSettingService.UpdateConfigVersion();

            if (appSettingService.FirstApplicationStart)
            {
                actionService.ShowNewToApplicationWindow();
            }
            else if (configVersionWasUpdated)
            {
                actionService.ShowNewToVersionDialog(appSettingService.LastInstalledVersion, previouslyInstalledVersion);
            }
            else if (appSettingService.ShowTipsAtApplicationStart)
            {
                actionService.ShowRandomApplicationTip();
            }

            if (appSettingService.CheckForUpdateOnStart)
            {
                var aboutViewModel = ConfiguredServices.ServiceProvider.GetRequiredService<AboutViewModel>();
                aboutViewModel.CheckForUpdatesCommand.Execute(false);
            }
        }
    }
}
