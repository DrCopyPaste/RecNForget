using Ookii.Dialogs.Wpf;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RecNForget
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged
	{
		private RecordingService recordingService;
		private bool currentlyRecording = false;
		private bool currentlyNotRecording = true;
		private string taskBar_ProgressState = "Paused";

		#region bound values

		public string TaskBar_ProgressState
		{
			get
			{
				return taskBar_ProgressState;
			}
			set
			{
				taskBar_ProgressState = value;
				OnPropertyChanged();
			}
		}

		public bool CurrentlyRecording
		{
			get
			{
				return currentlyRecording;
			}

			set
			{
				currentlyRecording = value;
				OnPropertyChanged();
			}
		}

		public bool CurrentlyNotRecording
		{
			get
			{
				return currentlyNotRecording;
			}
			set
			{
				currentlyNotRecording = value;
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
				SetAppConfigSetting("PromptForFileName", value.ToString());
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
				SetAppConfigSetting("HotKey_StartStopRecording", value);
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
				SetAppConfigSetting("HotKey_OpenLastRecording", value);
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
				SetAppConfigSetting("HotKey_OpenOutputPath", value);
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
				SetAppConfigSetting("HotKey_SetFileNamePrefix", value);
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
				SetAppConfigSetting("HotKey_ToggleFileNamePromptMode", value);
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
				SetAppConfigSetting("HotKey_SetOutputPath", value);
				OnPropertyChanged();
			}
		}

		public string FilenamePrefix
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings["FilenamePrefix"];
			}

			set
			{
				SetAppConfigSetting("FilenamePrefix", value);
				OnPropertyChanged();
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
				SetAppConfigSetting("OutputPath", value);
				OnPropertyChanged();
			}
		}

		#endregion

		public MainWindow()
		{
			DataContext = this;
			InitializeComponent();

			this.recordingService = new RecordingService(
				startRecordingAction:() =>
				{
					CurrentlyRecording = true;
					CurrentlyNotRecording = false;
					ToggleRecordButton.Content = "Stop";
					TaskBar_ProgressState = "Error";
				},
				stopRecordingAction: () =>
				{
					CurrentlyRecording = false;
					CurrentlyNotRecording = true;
					ToggleRecordButton.Content = "Record";
					TaskBar_ProgressState = "Paused";
				});
		}

		#region configuration event handlers

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void ConfigureHotkey_StartStopRecording_Click(object sender, RoutedEventArgs e)
		{
			this.recordingService.Pause();

			var dialog = new HotkeyPromptWindow("Configure start/stop recording hotkey");

			if (dialog.ShowDialog() == true)
			{
				HotKey_StartStopRecording = dialog.HotkeysText;
			}

			this.recordingService.Resume();
		}

		private void ConfigureHotkey_OpenLastRecording_Click(object sender, RoutedEventArgs e)
		{
			this.recordingService.Pause();

			var dialog = new HotkeyPromptWindow("Configure open last recording hotkey");

			if (dialog.ShowDialog() == true)
			{
				HotKey_OpenLastRecording = dialog.HotkeysText;
			}

			this.recordingService.Resume();
		}

		private void ConfigureHotkey_OpenOutputPath_Click(object sender, RoutedEventArgs e)
		{
			this.recordingService.Pause();

			var dialog = new HotkeyPromptWindow("Configure open output path hotkey");

			if (dialog.ShowDialog() == true)
			{
				HotKey_OpenOutputPath = dialog.HotkeysText;
			}

			this.recordingService.Resume();
		}

		private void ConfigureHotkey_SetFileNamePrefix_Click(object sender, RoutedEventArgs e)
		{
			this.recordingService.Pause();

			var dialog = new HotkeyPromptWindow("Configure set file name prefix hotkey");

			if (dialog.ShowDialog() == true)
			{
				HotKey_SetFileNamePrefix = dialog.HotkeysText;
			}

			this.recordingService.Resume();
		}

		private void ConfigureHotkey_ToggleFileNamePromptMode_Click(object sender, RoutedEventArgs e)
		{
			this.recordingService.Pause();

			var dialog = new HotkeyPromptWindow("Configure toggle file name prompt mode hotkey");

			if (dialog.ShowDialog() == true)
			{
				HotKey_ToggleFileNamePromptMode = dialog.HotkeysText;
			}

			this.recordingService.Resume();
		}

		private void ConfigureHotkey_SetOutputPath_Click(object sender, RoutedEventArgs e)
		{
			this.recordingService.Pause();

			var dialog = new HotkeyPromptWindow("Configure set output path hotkey");

			if (dialog.ShowDialog() == true)
			{
				HotKey_SetOutputPath = dialog.HotkeysText;
			}

			this.recordingService.Resume();
		}

		private void FilenamePrefix_Changed(object sender, RoutedEventArgs e)
		{
			SetAppConfigSetting("FilenamePrefix", FilenamePrefix);
		}

		private void SetAppConfigSetting(string settingKey, string settingValue)
		{
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
			configuration.AppSettings.Settings[settingKey].Value = settingValue;
			configuration.Save();

			ConfigurationManager.RefreshSection("appSettings");
		}

		private void Configure_OutputPath_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new VistaFolderBrowserDialog();

			if (dialog.ShowDialog() == true)
			{
				OutputPath = dialog.SelectedPath;
			}
		}

		#endregion

		#region runtime event handlers

		private void RecordButton_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentlyRecording)
			{
				recordingService.StopRecording();
			}
			else
			{
				recordingService.StartRecording();
			}
		}

		#endregion
	}
}
