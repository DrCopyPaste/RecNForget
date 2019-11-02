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

namespace RecNForget
{
	/// <summary>
	/// Interaction logic for HotkeyPromptWindow.xaml
	/// </summary>
	public partial class HotkeyPromptWindow : Window
	{
		private Hook keyboardHook;

		public HotkeyPromptWindow(string title)
		{
			this.Icon = new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Img", "logo.png")));

			InitializeComponent();

			this.Title = title;

			this.keyboardHook = new Hook("Configure Hotkey Hook");
			this.keyboardHook.KeyDownEvent = this.KeyHookDown;
		}

		public string HotkeysText
		{
			get { return HotkeysTextBox.Text; }
			set { HotkeysTextBox.Text = value; }
		}

		public string HotkeysAppSetting
		{
			get;
			set;
		}

		private void KeyHookDown(KeyboardHookEventArgs e)
		{
			HotkeysText = HotkeyToStringTranslator.GetKeyboardHookEventArgsAsString(e);

			if (e.Key != Keys.None)
			{
				HotkeysAppSetting = e.ToString();
				HotkeysText = HotkeyToStringTranslator.GetKeyboardHookEventArgsAsString(e);

				this.keyboardHook.isPaused = true;
				DialogResult = true;
			}
		}
	}
}
