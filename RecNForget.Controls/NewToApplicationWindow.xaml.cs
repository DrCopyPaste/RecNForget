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
using RecNForget.Services.Helpers;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for NewToApplicationWindow.xaml
    /// </summary>
    public partial class NewToApplicationWindow : INotifyPropertyChanged
    {
        private readonly IApplicationHotkeyService hotkeyService;
        private IAppSettingService settingService;

        public NewToApplicationWindow(IApplicationHotkeyService hotkeyService, IAppSettingService settingService)
        {
            DataContext = this;
            InitializeComponent();

            this.Title = "New to RecNForget?";

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.hotkeyService = new DesignerApplicationHotkeyService();
                SettingService = new DesignerAppSettingService();
                return;
            }
            else
            {
                this.hotkeyService = hotkeyService;
                SettingService = settingService;

                var buttonGrid = HotkeyRenderer.GetHotkeyListAsButtonGrid(
                hotkeys: HotkeySettingTranslator.GetHotkeySettingAsList(SettingService.HotKey_StartStopRecording, string.Empty, string.Empty),
                buttonStyle: (Style)FindResource("HotkeyDisplayButton"),
                spacing: 6);

                HotkeyDisplay.Children.Add(buttonGrid);

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

        private void DisplayHotkey()
        {
            if (HotkeyDisplay.Children.Count > 0)
            {
                HotkeyDisplay.Children.Clear();
            }

            var buttonGrid = HotkeyRenderer.GetHotkeyListAsButtonGrid(
                hotkeys: HotkeySettingTranslator.GetHotkeySettingAsList(SettingService.HotKey_StartStopRecording, string.Empty, string.Empty),
                buttonStyle: (Style)FindResource("HotkeyDisplayButton"),
                spacing: 6);

            HotkeyDisplay.Children.Add(buttonGrid);
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
                this.hotkeyService.ResetAndReadHotkeysFromConfig();
                SettingService.HotKey_StartStopRecording = dialog.HotkeysAppSetting;
                DisplayHotkey();
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
    }
}
