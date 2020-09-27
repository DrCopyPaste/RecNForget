using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Notifications.Wpf.Core;
using RecNForget.Controls;
using RecNForget.Controls.Services;
using RecNForget.Help;
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
        private readonly NotificationManager _notificationManager = new NotificationManager();
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
            mainWindow = UnityHandler.UnityContainer.Resolve<MainWindow>();

            actionService = UnityHandler.UnityContainer.Resolve<IActionService>();
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

                if (!appSettingService.MinimizedToTray && actionService.OwnerControl != null)
                {
                    dia.Owner = (Window)actionService.OwnerControl;
                }

                dia.Show();
            }
            else if (currentFileVersion.CompareTo(lastInstalledVersion) > 0)
            {
                var newToVersionDialog = new NewToVersionDialog(lastInstalledVersion, currentFileVersion, appSettingService);

                if (!appSettingService.MinimizedToTray && actionService.OwnerControl != null)
                {
                    newToVersionDialog.Owner = (Window)actionService.OwnerControl;
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
            var randomTip = HelpFeature.GetRandomFeature();

            int rowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Did you know?");
            sb.AppendLine();

            foreach (var line in randomTip.HelpLines)
            {
                if (rowCount > 3) break;

                sb.AppendLine(line.Content);
                rowCount++;
            }

            if (rowCount < randomTip.HelpLines.Count)
            {
                sb.AppendLine();
                sb.AppendLine("... (click to read more)");
            }

            _notificationManager.ShowAsync(
                content: new NotificationContent()
                {
                    Title = randomTip.Title,
                    Message = sb.ToString(),
                    Type = NotificationType.Information
                },
                expirationTime: TimeSpan.FromSeconds(10),
                onClick: () =>
                {
                    var quickTip = new QuickTipDialog(appSettingService, randomTip);

                    if (!appSettingService.MinimizedToTray && actionService.OwnerControl != null)
                    {
                        quickTip.Owner = (Window)actionService.OwnerControl;
                    }

                    quickTip.Show();
                });
        }
    }
}
