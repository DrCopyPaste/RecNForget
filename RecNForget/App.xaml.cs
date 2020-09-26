using System;
using System.Threading.Tasks;
using System.Windows;
using RecNForget.Controls;
using RecNForget.Controls.Services;
using RecNForget.IoC;
using RecNForget.Services;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services;
using RecNForget.WPF.Services.Contracts;
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
            UnityHandler.CreateContainer();
            var appSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();

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

            // register this application specific action service
            UnityHandler.UnityContainer.RegisterType<IActionService, ActionService>(lifetimeManager: new SingletonLifetimeManager());

            // ensure AppConfig Values exist
            bool firstTimeUser = appSettingService.RestoreDefaultAppConfigSetting(settingKey: null, overrideSetting: false);

            var hotkeyService = UnityHandler.UnityContainer.Resolve<IApplicationHotkeyService>();

            // Show main window first, so that windows popping up (like new updates/new to app) are in foreground and escapable
            MainWindow mainWindow = UnityHandler.UnityContainer.Resolve<MainWindow>();

            var actionService = UnityHandler.UnityContainer.Resolve<IActionService>();
            actionService.OwnerControl = mainWindow;

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
