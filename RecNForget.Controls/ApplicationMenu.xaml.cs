using RecNForget.Controls.Helper;
using RecNForget.Controls.IoC;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.WPF.Services.Contracts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unity;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for ApplicationMenu.xaml
    /// </summary>
    public partial class ApplicationMenu : INotifyPropertyChanged
    {
        private IAppSettingService appSettingService;

        private string currentSelectedThemeDisplayName;

        public string CurrentSelectedThemeDisplayName
        {
            get { return currentSelectedThemeDisplayName; }
            set { currentSelectedThemeDisplayName = value; OnPropertyChanged(); }
        }

        private IActionService actionService;

        public IAppSettingService AppSettingService
        {
            get { return appSettingService; }
            set { appSettingService = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ApplicationMenu()
        {
            InitializeComponent();
            DataContext = this;
            UserControlContextMenu.DataContext = this;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.AppSettingService = new DesignerAppSettingService();
                this.actionService = new DesignerActionService();
                return;
            }
            else
            {
                AppSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
                actionService = UnityHandler.UnityContainer.Resolve<IActionService>();

                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;
            }

            foreach (var theme in ThemeManager.GetAllThemeNames())
            {
                var isCurrentTheme = appSettingService.WindowTheme.ToUpper() == theme.Key.ToUpper();
                if (isCurrentTheme) CurrentSelectedThemeDisplayName = theme.Value;

                var item = new System.Windows.Controls.MenuItem()
                {
                    Header = theme.Value,
                    IsCheckable = true,
                    IsChecked = isCurrentTheme
                };
                item.Click += (object sender, RoutedEventArgs e) => { ThemeManager.ChangeTheme(theme.Key); appSettingService.WindowTheme = theme.Key; };
                ThemeNode.Items.Add(item);
            }

            this.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
            this.ContextMenu.IsOpen = true;
        }

        private void Settings_MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            actionService.ShowSettingsMenu();
        }

        private void About_MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            actionService.ShowAboutWindow();
        }

        private void Help_MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            actionService.ShowHelpWindow();
        }
        
        private void CheckForUpdates_MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            actionService.CheckForUpdates(showMessages: true);
        }

        private void Exit_MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            actionService.Exit();
        }
    }
}
