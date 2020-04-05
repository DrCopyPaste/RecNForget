using System;
using System.Windows;
using RecNForget.Services;
using RecNForget.Services.Contracts;
using RecNForget.Windows;
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

            UnityContainer unityContainer = new UnityContainer();

            unityContainer.RegisterType<IAppSettingService, AppSettingService>(lifetimeManager: new SingletonLifetimeManager());
            //unityContainer.RegisterType<IMainWindow, MainWindow>();

            // ToDo MainWindow constructor code should not be necessary to start hotkey and recording services...
            MainWindow mainWindow = unityContainer.Resolve<MainWindow>();           
        }
    }
}
