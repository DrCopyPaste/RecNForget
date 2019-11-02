using Ookii.Dialogs.Wpf;
using RecNForget.Enums;
using RecNForget.Services;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RecNForget
{
	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : INotifyPropertyChanged
	{
		private HotkeyService hotkeyService;

		public SettingsWindow(HotkeyService hotkeyService)
		{
			this.hotkeyService = hotkeyService;

			DataContext = this;
			InitializeComponent();
		}

		public string HotKey_StartStopRecording
		{
			get
			{
				return HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_StartStopRecording");
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("HotKey_StartStopRecording", value);
				OnPropertyChanged();
			}
		}

		public string HotKey_OpenLastRecording
		{
			get
			{
				return HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_OpenLastRecording");
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("HotKey_OpenLastRecording", value);
				OnPropertyChanged();
			}
		}

		public string HotKey_OpenOutputPath
		{
			get
			{
				return HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_OpenOutputPath");
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("HotKey_OpenOutputPath", value);
				OnPropertyChanged();
			}
		}

		public string HotKey_SetFileNamePrefix
		{
			get
			{
				return HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_SetFileNamePrefix");
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("HotKey_SetFileNamePrefix", value);
				OnPropertyChanged();
			}
		}

		public string HotKey_ToggleFileNamePromptMode
		{
			get
			{
				return HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_ToggleFileNamePromptMode");
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("HotKey_ToggleFileNamePromptMode", value);
				OnPropertyChanged();
			}
		}

		public string HotKey_SetOutputPath
		{
			get
			{
				return HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_SetOutputPath");
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("HotKey_SetOutputPath", value);
				OnPropertyChanged();
			}
		}

		public PromptForFilenameMode PromptForFilename
		{
			get
			{
				return (PromptForFilenameMode)Enum.Parse(typeof(PromptForFilenameMode), System.Configuration.ConfigurationManager.AppSettings["PromptForFileName"]);
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("PromptForFileName", value.ToString());
			}
		}

		public string OutputPath
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings["OutputPath"];
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("OutputPath", value);
				OnPropertyChanged();
			}
		}

		#region configuration event handlers

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region runtime event handlers

		private void ConfigureHotkey_StartStopRecording_Click(object sender, RoutedEventArgs e)
		{
			this.hotkeyService.PauseCapturingHotkeys();

			var dialog = new HotkeyPromptWindow("Configure start/stop recording hotkey");

			if (dialog.ShowDialog() == true)
			{
				HotKey_StartStopRecording = dialog.HotkeysAppSetting;
			}

			this.hotkeyService.ResumeCapturingHotkeys();
		}

		//private void ConfigureHotkey_OpenLastRecording_Click(object sender, RoutedEventArgs e)
		//{
		//	this.hotkeyService.PauseCapturingHotkeys();

		//	var dialog = new HotkeyPromptWindow("Configure open last recording hotkey");

		//	if (dialog.ShowDialog() == true)
		//	{
		//		HotKey_OpenLastRecording = dialog.HotkeysAppSetting;
		//	}

		//	this.hotkeyService.ResumeCapturingHotkeys();
		//}

		//private void ConfigureHotkey_OpenOutputPath_Click(object sender, RoutedEventArgs e)
		//{
		//	this.hotkeyService.PauseCapturingHotkeys();

		//	var dialog = new HotkeyPromptWindow("Configure open output path hotkey");

		//	if (dialog.ShowDialog() == true)
		//	{
		//		HotKey_OpenOutputPath = dialog.HotkeysAppSetting;
		//	}

		//	this.hotkeyService.ResumeCapturingHotkeys();
		//}

		//private void ConfigureHotkey_SetFileNamePrefix_Click(object sender, RoutedEventArgs e)
		//{
		//	this.hotkeyService.PauseCapturingHotkeys();

		//	var dialog = new HotkeyPromptWindow("Configure set file name prefix hotkey");

		//	if (dialog.ShowDialog() == true)
		//	{
		//		HotKey_SetFileNamePrefix = dialog.HotkeysAppSetting;
		//	}

		//	this.hotkeyService.ResumeCapturingHotkeys();
		//}

		//private void ConfigureHotkey_ToggleFileNamePromptMode_Click(object sender, RoutedEventArgs e)
		//{
		//	this.hotkeyService.PauseCapturingHotkeys();

		//	var dialog = new HotkeyPromptWindow("Configure toggle file name prompt mode hotkey");

		//	if (dialog.ShowDialog() == true)
		//	{
		//		HotKey_ToggleFileNamePromptMode = dialog.HotkeysAppSetting;
		//	}

		//	this.hotkeyService.ResumeCapturingHotkeys();
		//}

		//private void ConfigureHotkey_SetOutputPath_Click(object sender, RoutedEventArgs e)
		//{
		//	this.hotkeyService.PauseCapturingHotkeys();

		//	var dialog = new HotkeyPromptWindow("Configure set output path hotkey");

		//	if (dialog.ShowDialog() == true)
		//	{
		//		HotKey_SetOutputPath = dialog.HotkeysAppSetting;
		//	}

		//	this.hotkeyService.ResumeCapturingHotkeys();
		//}

		private void Configure_OutputPath_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new VistaFolderBrowserDialog();

			if (dialog.ShowDialog() == true)
			{
				OutputPath = dialog.SelectedPath;
			}
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		#endregion
	}
}
