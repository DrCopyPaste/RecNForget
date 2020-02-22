using Ookii.Dialogs.Wpf;
using RecNForget.Controls;
using RecNForget.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RecNForget.Windows
{
	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : INotifyPropertyChanged
	{
		private Action hideToTrayAction;
		private Action restoreFromTrayAction;

		private HotkeyService hotkeyService;
		private AppSettingService settingService;
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

		public SettingsWindow(HotkeyService hotkeyService, Action hideToTrayAction, Action restoreFromTrayAction)
		{
			this.hotkeyService = hotkeyService;
			this.SettingService = new AppSettingService();
			this.hideToTrayAction = hideToTrayAction;
			this.restoreFromTrayAction = restoreFromTrayAction;

			DataContext = this;
			InitializeComponent();

			DisplayHotkey();
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
				spacing: 6,
				horizontalAlignment: HorizontalAlignment.Left);

			HotkeyDisplay.Children.Add(buttonGrid);
		}

		#region configuration event handlers

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region runtime event handlers

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
		}

		private void ConfigureHotkey_StartStopRecording_Click(object sender, RoutedEventArgs e)
		{
			this.hotkeyService.PauseCapturingHotkeys();

			var dialog = new HotkeyPromptWindow("Configure start/stop recording hotkey");

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

		private void Configure_FileNamePattern_Click(object sender, RoutedEventArgs e)
		{
			CustomMessageBox tempDialog = new CustomMessageBox(
				caption: "Type in a new pattern for file name generation.",
				icon: CustomMessageBoxIcon.Question,
				buttons: CustomMessageBoxButtons.OkAndCancel,
				messageRows: new List<string>() { "Supported placeholders:", "(Date)" },
				prompt: SettingService.FilenamePrefix,
				controlFocus: CustomMessageBoxFocus.Prompt,
				promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

			if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
			{
				SettingService.FilenamePrefix = tempDialog.PromptContent;
			}

			// since there are two buttons on top of each other
			e.Handled = true;
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		#endregion
	}
}
