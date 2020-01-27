using FMUtils.KeyboardHook;
using Ookii.Dialogs.Wpf;
using RecNForget.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
	/// Interaction logic for NewToApplicationWindow.xaml
	/// </summary>
	public partial class NewToApplicationWindow : INotifyPropertyChanged
	{
		private HotkeyService hotkeyService;

		public NewToApplicationWindow(HotkeyService hotkeyService)
		{
			this.hotkeyService = hotkeyService;
			this.KeyDown += Window_KeyDown;

			InitializeComponent();
			DataContext = this;

			this.Title = "New to RecNForget?";

			var buttonGrid = HotkeyToStringTranslator.GetHotkeyListAsButtonGrid(
				hotkeys: HotkeyToStringTranslator.GetHotkeySettingAsList(AppSettingHelper.HotKey_StartStopRecording, string.Empty, string.Empty),
				buttonStyle: (Style)FindResource("HotkeyDisplayButton"),
				spacing: 6);

			HotkeyDisplay.Children.Add(buttonGrid);
		}

		private void DisplayHotkey()
		{
			if (HotkeyDisplay.Children.Count > 0)
			{
				HotkeyDisplay.Children.Clear();
			}

			var buttonGrid = HotkeyToStringTranslator.GetHotkeyListAsButtonGrid(
				hotkeys: HotkeyToStringTranslator.GetHotkeySettingAsList(AppSettingHelper.HotKey_StartStopRecording, string.Empty, string.Empty),
				buttonStyle: (Style)FindResource("HotkeyDisplayButton"),
				spacing: 6);

			HotkeyDisplay.Children.Add(buttonGrid);
		}

		public string HotKey_StartStopRecording
		{
			get
			{
				return HotkeyToStringTranslator.GetHotkeySettingAsString(AppSettingHelper.HotKey_StartStopRecording);
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.HotKey_StartStopRecording, value);
				OnPropertyChanged();
			}
		}

		public string OutputPath
		{
			get
			{
				return AppSettingHelper.GetAppConfigSetting(AppSettingHelper.OutputPath);
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.OutputPath, value);
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
			this.hotkeyService.PauseCapturingHotkeys();

			var dialog = new HotkeyPromptWindow("Configure start/stop recording hotkey")
			{
				Owner = this
			};

			if (dialog.ShowDialog() == true)
			{
				HotKey_StartStopRecording = dialog.HotkeysAppSetting;
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
				OutputPath = dialog.SelectedPath;
			}

			// since there are two buttons on top of each other
			e.Handled = true;
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		#region configuration event handlers

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
