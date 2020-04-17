using RecNForget.Services;
using RecNForget.Services.Contracts;
using Unity;
using Unity.Lifetime;

namespace RecNForget.IoC
{
    public class UnityHandler
    {
        private static UnityContainer unityContainer;

        public static UnityContainer UnityContainer
        {
            get
            {
                if (unityContainer == null)
                {
                    CreateContainer();
                }

                return unityContainer;
            }
        }

        public static void CreateContainer()
        {
            unityContainer = new UnityContainer();

            UnityContainer.RegisterType<IAppSettingService, AppSettingService>(lifetimeManager: new SingletonLifetimeManager());
            UnityContainer.RegisterType<ISelectedFileService, SelectedFileService>(lifetimeManager: new SingletonLifetimeManager());
            UnityContainer.RegisterType<IAudioPlaybackService, AudioPlaybackService>(lifetimeManager: new SingletonLifetimeManager());
            UnityContainer.RegisterType<IHotkeyService, HotkeyService>(lifetimeManager: new SingletonLifetimeManager());
            UnityContainer.RegisterType<IAudioRecordingService, AudioRecordingService>(lifetimeManager: new SingletonLifetimeManager());
            // UnityContainer.RegisterType<IActionService, ActionService>(lifetimeManager: new SingletonLifetimeManager());
            // UnityContainer.RegisterType<IMainWindow, MainWindow>();
        }
    }
}
