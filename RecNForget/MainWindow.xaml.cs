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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged
	{
		private HotkeyService hotkeyService;
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
				AppSettingHelper.SetAppConfigSetting("PromptForFileName", value.ToString());
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
				AppSettingHelper.SetAppConfigSetting("FilenamePrefix", value);
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
				AppSettingHelper.SetAppConfigSetting("OutputPath", value);
				OnPropertyChanged();
			}
		}

		#endregion

		public MainWindow()
		{
			DataContext = this;
			InitializeComponent();

			this.hotkeyService = new HotkeyService(
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

		private void FilenamePrefix_Changed(object sender, RoutedEventArgs e)
		{
			AppSettingHelper.SetAppConfigSetting("FilenamePrefix", FilenamePrefix);
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
				hotkeyService.StopRecording();
			}
			else
			{
				hotkeyService.StartRecording();
			}
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			var settingsWindow = new SettingsWindow(hotkeyService);
			settingsWindow.ShowDialog();
		}

		#endregion
	}
}
