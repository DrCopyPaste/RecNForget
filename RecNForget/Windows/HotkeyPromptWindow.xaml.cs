using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using FMUtils.KeyboardHook;
using RecNForget.Services;

namespace RecNForget.Windows
{
    /// <summary>
    /// Interaction logic for HotkeyPromptWindow.xaml
    /// </summary>
    public partial class HotkeyPromptWindow : Window
    {
        private Hook keyboardHook;

        public HotkeyPromptWindow(string title)
        {
            InitializeComponent();

            this.Title = title;
            this.keyboardHook = new Hook("Configure Hotkey Hook");
            this.keyboardHook.KeyDownEvent = this.KeyHookDown;
        }

        public string HotkeysAppSetting
        {
            get;
            set;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void KeyHookDown(KeyboardHookEventArgs e)
        {
            if (HotkeyDisplay.Children.Count > 0)
            {
                HotkeyDisplay.Children.Clear();
            }

            var buttonGrid = HotkeySettingTranslator.GetHotkeyListAsButtonGrid(
                hotkeys: HotkeySettingTranslator.GetKeyboardHookEventArgsAsList(e, string.Empty, string.Empty),
                buttonStyle: (Style)FindResource("HotkeyDisplayButton"),
                spacing: 6);

            HotkeyDisplay.Children.Add(buttonGrid);

            if (e.Key != Keys.None)
            {
                HotkeysAppSetting = e.ToString();
                this.keyboardHook.isPaused = true;

                DialogResult = true;
            }
        }
    }
}