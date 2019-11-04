using Hardcodet.Wpf.TaskbarNotification;
using NAudio.Wave;
using Ookii.Dialogs.Wpf;
using RecNForget.Enums;
using RecNForget.Services;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RecNForget
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged
	{
		private TaskbarIcon taskBarIcon;

		private WaveOutEvent audioOutputDevice = null;
		private AudioFileReader audioFileReader = null;

		private HotkeyService hotkeyService;
		private bool currentlyRecording = false;
		private bool currentlyNotRecording = true;
		private bool currentAudioPlayState = true;
		private string taskBar_ProgressState = "Paused";

		private bool hasLastRecording = false;
		private string currentFileName;
		private string lastFileName;

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

		public bool WindowAlwaysOnTop
		{
			get
			{
				return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["WindowAlwaysOnTop"]);
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting("WindowAlwaysOnTop", value.ToString());
				this.Topmost = value;
				OnPropertyChanged();
			}
		}

		public string CurrentFileName
		{
			get
			{
				return currentFileName == string.Empty ? "(not recording)" : currentFileName;
			}

			set
			{
				currentFileName = value;
				OnPropertyChanged();
			}
		}

		public bool CurrentAudioPlayState
		{
			get
			{
				return currentAudioPlayState;
			}
			set
			{
				currentAudioPlayState = value;
				OnPropertyChanged();
			}
		}

		public bool HasLastRecording
		{
			get
			{
				return hasLastRecording;
			}
			set
			{
				hasLastRecording = value;
				OnPropertyChanged();
			}
		}

		public string LastFileName
		{
			get
			{
				return lastFileName == string.Empty ? "(nothing)" : lastFileName;
			}

			set
			{
				lastFileName = value;

				if (!string.IsNullOrWhiteSpace(lastFileName))
				{
					HasLastRecording = true;
				}
				else
				{
					HasLastRecording = false;
				}

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
		}

		#endregion

		public MainWindow()
		{
			taskBarIcon = new TaskbarIcon();
			this.Icon = new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Img", "logo.png")));
			this.Topmost = WindowAlwaysOnTop;

			DataContext = this;
			InitializeComponent();

			this.hotkeyService = new HotkeyService(
				startRecordingAction:() =>
				{
					CurrentlyRecording = true;
					CurrentlyNotRecording = false;
					ToggleRecordButton.Content = "Stop";
					TaskBar_ProgressState = "Error";
					UpdateCurrentFileName();
					taskBarIcon.ShowBalloonTip("Recording started!", string.Format("RecNForget now recording to {0}", CurrentFileName), BalloonIcon.Info);
				},
				stopRecordingAction: () =>
				{
					CurrentlyRecording = false;
					CurrentlyNotRecording = true;
					ToggleRecordButton.Content = "Record";
					TaskBar_ProgressState = "Paused";
					UpdateCurrentFileName();
					UpdateLastFileName();

					// stop recording is somehow triggered during service start?
					if (LastFileName != "(nothing)")
					{
						taskBarIcon.ShowBalloonTip("Recording saved!", string.Format("RecNForget saved to {0}", LastFileName), BalloonIcon.Info);
					}
				});

			ToggleRecordButton.Focus();
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

		private void UpdateCurrentFileName()
		{
			CurrentFileName = hotkeyService == null ? string.Empty : hotkeyService.CurrentFileName;
		}

		private void UpdateLastFileName()
		{
			LastFileName = hotkeyService == null ? string.Empty : hotkeyService.LastFileName;
		}

		private void OpenOutputFolder_Click(object sender, RoutedEventArgs e)
		{
			Process.Start(OutputPath);
		}

		// https://github.com/naudio/NAudio/blob/master/Docs/PlayAudioFileWinForms.md
		private void ReplayLastRecording_Click(object sender, RoutedEventArgs e)
		{
			if (audioOutputDevice == null || audioOutputDevice.PlaybackState == PlaybackState.Stopped)
			{
				audioOutputDevice = new WaveOutEvent();
				audioOutputDevice.PlaybackStopped += OnPlaybackStopped;

				audioFileReader = new AudioFileReader(LastFileName);
				audioOutputDevice.Init(audioFileReader);
				audioOutputDevice.Play();

				CurrentAudioPlayState = true;
				ReplayLastRecordingButton.Content = "Pause";
			}
			else if (audioOutputDevice.PlaybackState == PlaybackState.Paused)
			{
				audioOutputDevice.Play();

				CurrentAudioPlayState = true;
				ReplayLastRecordingButton.Content = "Pause";
			}
			else if (audioOutputDevice.PlaybackState == PlaybackState.Playing)
			{
				audioOutputDevice.Pause();

				CurrentAudioPlayState = false;
				ReplayLastRecordingButton.Content = "Resume";
			}
		}

		// https://github.com/naudio/NAudio/blob/master/Docs/PlayAudioFileWinForms.md
		private void OnPlaybackStopped(object sender, StoppedEventArgs args)
		{
			audioOutputDevice.Dispose();
			audioOutputDevice = null;
			audioFileReader.Dispose();
			audioFileReader = null;

			CurrentAudioPlayState = false;
			ReplayLastRecordingButton.Content = "Replay Last";
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			var settingsWindow = new SettingsWindow(hotkeyService);
			settingsWindow.ShowDialog();
		}

		#endregion
	}
}
