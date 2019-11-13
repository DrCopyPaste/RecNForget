using Ookii.Dialogs.Wpf;
using RecNForget.Services;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
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

			OkButton.Focus();
		}

		public bool AutoStartWithWindows
		{
			get
			{
				Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				var recNForgetAutoStartRegistry = regKey.GetValue("RecNForget");
					
				if (recNForgetAutoStartRegistry != null)
				{
					return true;
				}

				return false;
			}

			set
			{
				Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

				if (value == true)
				{
					regKey.SetValue("RecNForget", System.Reflection.Assembly.GetExecutingAssembly().Location);
				}
				else
				{
					regKey.DeleteValue("RecNForget");
				}

				OnPropertyChanged();
			}
		}

		public bool MinimizedToTray
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting("MinimizedToTray"));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("MinimizedToTray", value.ToString());
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

		public bool PlayAudioFeedBackMarkingStartAndStopReplaying
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting("PlayAudioFeedBackMarkingStartAndStopReplaying"));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("PlayAudioFeedBackMarkingStartAndStopReplaying", value.ToString());
				OnPropertyChanged();
			}
		}

		public bool AutoReplayAudioAfterRecording
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting("AutoReplayAudioAfterRecording"));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("AutoReplayAudioAfterRecording", value.ToString());
				OnPropertyChanged();
			}
		}

		public bool PlayAudioFeedBackMarkingStartAndStopRecording
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting("PlayAudioFeedBackMarkingStartAndStopRecording"));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("PlayAudioFeedBackMarkingStartAndStopRecording", value.ToString());
				OnPropertyChanged();
			}
		}

		public bool ShowBalloonTipsForRecording
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting("ShowBalloonTipsForRecording"));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("ShowBalloonTipsForRecording", value.ToString());
				OnPropertyChanged();
			}
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

		public string OutputPath
		{
			get
			{
				return AppSettingHelper.GetAppConfigSetting("OutputPath");
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
