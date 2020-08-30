using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using FMUtils.KeyboardHook;
using RecNForget.Controls.Helper;
using RecNForget.Services;
using RecNForget.Services.Helpers;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for HotkeyPromptWindow.xaml
    /// </summary>
    public partial class HotkeyPromptWindow : Window
    {
        public HotkeyPromptWindow(string title)
        {
            DataContext = this;
            InitializeComponent();

            this.Title = title;
            this.KeyDown += HotkeyPromptWindow_KeyDown;
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

        private void HotkeyPromptWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var currentModifiers = Keyboard.Modifiers;

            if (HotkeyDisplay.Children.Count > 0)
            {
                HotkeyDisplay.Children.Clear();
            }

            var buttonGrid = HotkeyRenderer.GetHotkeyListAsButtonGrid(
                hotkeys: HotkeySettingTranslator.GetKeyEventArgsAsList(e, currentModifiers, string.Empty, string.Empty),
                buttonStyle: (Style)FindResource("HotkeyDisplayButton"),
                spacing: 6);

            HotkeyDisplay.Children.Add(buttonGrid);

            if (e.Key != Key.None && !HotkeySettingTranslator.ModifierKeys.Contains(e.Key))
            {
                try
                {
                    KeyGesture gesture = new KeyGesture(e.Key, currentModifiers);
                    HotkeysAppSetting = HotkeySettingTranslator.GetSettingStringFromKeyGesture(gesture);

                    DialogResult = true;
                }
                catch (NotSupportedException ex)
                {
                    // silently ignore this
                    // this means we cannot assign this hotkey without modifiers (this is true for most keys like A-Z, but not for others like F1-F12 or Pause, etc.)
                }
            }
        }
    }
}