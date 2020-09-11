﻿using System;
using System.Threading.Tasks;
using System.Windows;
using RecNForget.Controls;
using RecNForget.Controls.Services;
using RecNForget.IoC;
using RecNForget.Services;
using RecNForget.Services.Contracts;
using Unity;
using Unity.Lifetime;

namespace RecNForget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            UnityHandler.CreateContainer();

            var appSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
            var hotkeyService = UnityHandler.UnityContainer.Resolve<IApplicationHotkeyService>();

            var actionService = new ActionService();

            // ensure AppConfig Values exist
            bool firstTimeUser = appSettingService.RestoreDefaultAppConfigSetting(settingKey: null, overrideSetting: false);

            // Show main window first, so that windows popping up (like new updates/new to app) are in foreground and escapable
            MainWindow mainWindow = UnityHandler.UnityContainer.Resolve<MainWindow>();

            HandleFirstStartAndUpdates(actionService, appSettingService, hotkeyService, firstTimeUser);
        }

        private void HandleFirstStartAndUpdates(IActionService actionService, IAppSettingService appSettingService, IApplicationHotkeyService hotkeyService, bool firstTimeUser)
        {
            if (appSettingService.CheckForUpdateOnStart)
            {
                Task.Run(() => { actionService.CheckForUpdates(showMessages: false); });
            }

            var currentFileVersion = new Version(ThisAssembly.AssemblyFileVersion);
            Version lastInstalledVersion = appSettingService.LastInstalledVersion;

            appSettingService.LastInstalledVersion = currentFileVersion;

            if (firstTimeUser)
            {
                var dia = new NewToApplicationWindow(hotkeyService, appSettingService);

                if (!appSettingService.MinimizedToTray)
                {
                    // dia.Owner = this;
                }

                dia.Show();
            }
            else if (currentFileVersion.CompareTo(lastInstalledVersion) > 0)
            {
                var newToVersionDialog = new NewToVersionDialog(lastInstalledVersion, currentFileVersion, appSettingService);

                if (!appSettingService.MinimizedToTray)
                {
                    // newToVersionDialog.Owner = this;
                }

                newToVersionDialog.Show();
            }
            else if (appSettingService.ShowTipsAtApplicationStart)
            {
                ShowRandomApplicationTip(appSettingService);
            }
        }

        private void ShowRandomApplicationTip(IAppSettingService appSettingService)
        {
            var quickTip = new QuickTipDialog(appSettingService);

            if (!appSettingService.MinimizedToTray)
            {
                // quickTip.Owner = this;
            }

            quickTip.Show();
        }
    }
}
