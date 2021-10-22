using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Ookii.Dialogs.Wpf;
using RecNForget.Controls;
using RecNForget.Controls.Helper;
using RecNForget.Services;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.WPF.Services.Contracts;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for NewToApplicationWindow.xaml
    /// </summary>
    public partial class NewToApplicationWindow : INotifyPropertyChanged
    {
        private readonly IApplicationHotkeyService hotkeyService;
        private IAppSettingService settingService;
        private IActionService actionService;

        public NewToApplicationWindow(IApplicationHotkeyService hotkeyService, IAppSettingService settingService, IActionService actionService)
        {
            InitializeComponent();

            this.Title = "New to RecNForget?";

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.hotkeyService = new DesignerApplicationHotkeyService();
                this.actionService = new DesignerActionService();
                SettingService = new DesignerAppSettingService();
                return;
            }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;

                this.hotkeyService = hotkeyService;
                SettingService = settingService;
                this.actionService = actionService;

                this.KeyDown += Window_KeyDown;
            }            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IAppSettingService SettingService
        {
            get
            {
                return settingService;
            }

            set
            {
                settingService = value;
                OnPropertyChanged();
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void ConfigureHotkey_StartStopRecording_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new HotkeyPromptWindow("Configure start/stop recording hotkey")
            {
                Owner = this
            };

            if (dialog.ShowDialog() == true)
            {
                SettingService.HotKey_StartStopRecording = dialog.HotkeysAppSetting;
                this.hotkeyService.ResetAndReadHotkeysFromConfig();
            }

            // since there are two buttons on top of each other
            e.Handled = true;
        }

        private void Configure_OutputPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();

            if (dialog.ShowDialog() == true)
            {
                SettingService.OutputPath = dialog.SelectedPath;
            }

            // since there are two buttons on top of each other
            e.Handled = true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            actionService.ShowSettingsMenu();
        }
    }
}
