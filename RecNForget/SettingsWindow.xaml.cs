using Ookii.Dialogs.Wpf;
using RecNForget.Services;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RecNForget
{
	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : INotifyPropertyChanged
	{
		private HotkeyService hotkeyService;
		private Action hideToTrayAction;
		private Action restoreFromTrayAction;

		public SettingsWindow(HotkeyService hotkeyService, Action hideToTrayAction, Action restoreFromTrayAction)
		{
			this.hotkeyService = hotkeyService;
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

			var buttonGrid = HotkeyToStringTranslator.GetHotkeyListAsButtonGrid(
				hotkeys: HotkeyToStringTranslator.GetHotkeySettingAsList(AppSettingHelper.HotKey_StartStopRecording, string.Empty, string.Empty),
				buttonStyle: (Style)FindResource("HotkeyDisplayButton"),
				spacing: 6);

			HotkeyDisplay.Children.Add(buttonGrid);
		}

		public bool AutoStartWithWindows
		{
			get
			{
				Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(AppSettingHelper.WindowsAutoStartRegistryPath, true);
				var recNForgetAutoStartRegistry = regKey.GetValue(AppSettingHelper.ApplicationName);
					
				if (recNForgetAutoStartRegistry != null)
				{
					return true;
				}

				return false;
			}

			set
			{
				Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(AppSettingHelper.WindowsAutoStartRegistryPath, true);

				if (value == true)
				{
					regKey.SetValue(AppSettingHelper.ApplicationName, System.Reflection.Assembly.GetExecutingAssembly().Location);
				}
				else
				{
					regKey.DeleteValue(AppSettingHelper.ApplicationName);
				}

				OnPropertyChanged();
			}
		}

		public bool MinimizedToTray
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.MinimizedToTray));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.MinimizedToTray, value.ToString());
				OnPropertyChanged();

				if (value == true)
				{
					hideToTrayAction();
				}
				else
				{
					restoreFromTrayAction();
				}
			}
		}

		public bool AutoSelectLastRecording
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.AutoSelectLastRecording));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.AutoSelectLastRecording, value.ToString());
				OnPropertyChanged();
			}
		}

		public bool CheckForUpdateOnStart
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.CheckForUpdateOnStart));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.CheckForUpdateOnStart, value.ToString());
				OnPropertyChanged();
			}
		}

		public bool PlayAudioFeedBackMarkingStartAndStopReplaying
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.PlayAudioFeedBackMarkingStartAndStopReplaying));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.PlayAudioFeedBackMarkingStartAndStopReplaying, value.ToString());
				OnPropertyChanged();
			}
		}

		public bool AutoReplayAudioAfterRecording
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.AutoReplayAudioAfterRecording));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.AutoReplayAudioAfterRecording, value.ToString());
				OnPropertyChanged();
			}
		}

		public bool PlayAudioFeedBackMarkingStartAndStopRecording
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.PlayAudioFeedBackMarkingStartAndStopRecording));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.PlayAudioFeedBackMarkingStartAndStopRecording, value.ToString());
				OnPropertyChanged();
			}
		}

		public bool ShowBalloonTipsForRecording
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.ShowBalloonTipsForRecording));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.ShowBalloonTipsForRecording, value.ToString());
				OnPropertyChanged();
			}
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
				HotKey_StartStopRecording = dialog.HotkeysAppSetting;
				DisplayHotkey();
			}

			this.hotkeyService.ResumeCapturingHotkeys();
		}

		private void Configure_OutputPath_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new VistaFolderBrowserDialog();

			if (dialog.ShowDialog() == true)
			{
				OutputPath = dialog.SelectedPath;
			}
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
