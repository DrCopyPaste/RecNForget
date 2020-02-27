using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Ookii.Dialogs.Wpf;
using RecNForget.Services;

namespace RecNForget.Windows
{
    /// <summary>
    /// Interaction logic for NewToApplicationWindow.xaml
    /// </summary>
    public partial class NewToApplicationWindow : INotifyPropertyChanged
    {
        private HotkeyService hotkeyService;
        private AppSettingService settingService;

        public NewToApplicationWindow(HotkeyService hotkeyService)
        {
            this.hotkeyService = hotkeyService;
            this.SettingService = new AppSettingService();
            this.KeyDown += Window_KeyDown;

            InitializeComponent();
            DataContext = this;

            this.Title = "New to RecNForget?";

            var buttonGrid = HotkeySettingTranslator.GetHotkeyListAsButtonGrid(
                hotkeys: HotkeySettingTranslator.GetHotkeySettingAsList(SettingService.HotKey_StartStopRecording, string.Empty, string.Empty),
                buttonStyle: (Style)FindResource("HotkeyDisplayButton"),
                spacing: 6);

            HotkeyDisplay.Children.Add(buttonGrid);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AppSettingService SettingService
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

            var buttonGrid = HotkeySettingTranslator.GetHotkeyListAsButtonGrid(
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
            this.hotkeyService.PauseCapturingHotkeys();

            var dialog = new HotkeyPromptWindow("Configure start/stop recording hotkey")
            {
                Owner = this
            };

            if (dialog.ShowDialog() == true)
            {
                SettingService.HotKey_StartStopRecording = dialog.HotkeysAppSetting;
                DisplayHotkey();
            }

            this.hotkeyService.ResumeCapturingHotkeys();

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
