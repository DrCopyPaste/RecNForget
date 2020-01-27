using FMUtils.KeyboardHook;
using RecNForget.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
				this.DragMove();
		}

		private void KeyHookDown(KeyboardHookEventArgs e)
		{
			if (HotkeyDisplay.Children.Count > 0)
			{
				HotkeyDisplay.Children.Clear();
			}
			
			var buttonGrid = HotkeyToStringTranslator.GetHotkeyListAsButtonGrid(
				hotkeys: HotkeyToStringTranslator.GetKeyboardHookEventArgsAsList(e, string.Empty, string.Empty),
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
;