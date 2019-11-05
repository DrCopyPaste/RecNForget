﻿using Hardcodet.Wpf.TaskbarNotification;
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
using System.Windows.Threading;

namespace RecNForget
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged
	{
		private string logoPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Img", "logo.png");

		private Icon applicationIcon;
		private TaskbarIcon taskBarIcon;

		private WaveOutEvent audioOutputDevice = null;
		private AudioFileReader audioFileReader = null;

		private HotkeyService hotkeyService;
		private bool currentlyRecording = false;
		private bool currentlyNotRecording = true;
		private bool currentAudioPlayState = true;
		private string taskBar_ProgressState = "Paused";

		private DispatcherTimer recordingTimer;

		private string recordingTimeInMilliSeconds = string.Empty;
		private bool hasLastRecording = false;
		private string currentFileName;
		private string lastFileName;

		public DateTime RecordingStart { get; set; }

		#region bound values

		public string RecordingTimeInMilliSeconds
		{
			get
			{
				return CurrentlyRecording ? recordingTimeInMilliSeconds : string.Empty;
			}
			set
			{
				recordingTimeInMilliSeconds = value;
				OnPropertyChanged();
			}
		}

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

		public string CurrentFileNameDisplay
		{
			get
			{
				return currentFileName;
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

		public bool MinimizedToTray
		{
			get
			{
				return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["MinimizedToTray"]);
			}
		}

		#endregion

		public MainWindow()
		{
			applicationIcon = getIcon();

			taskBarIcon = new TaskbarIcon();
			taskBarIcon.Icon = applicationIcon;
			taskBarIcon.DoubleClickCommand = new RestoreMainWindowFromTrayCommand(() => { SwitchToForegroundMode(); });

			recordingTimer = new DispatcherTimer();
			recordingTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
			recordingTimer.Tick += new EventHandler(RecordingTimer_Tick);

			if (MinimizedToTray)
			{
				SwitchToBackgroundMode();
			}
			else
			{
				SwitchToForegroundMode();
			}

			this.Icon = new BitmapImage(new Uri(logoPath));
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
					taskBarIcon.ShowBalloonTip("Recording started!", string.Format("RecNForget now recording to {0}", CurrentFileNameDisplay), applicationIcon, true);
					RecordingStart = DateTime.Now;
					recordingTimer.Start();
					UpdateCurrentFileNameDisplay();
				},
				stopRecordingAction: () =>
				{
					CurrentlyRecording = false;
					CurrentlyNotRecording = true;
					ToggleRecordButton.Content = "Record";
					TaskBar_ProgressState = "Paused";
					UpdateCurrentFileNameDisplay();
					UpdateLastFileName();
					taskBarIcon.ShowBalloonTip("Recording saved!", string.Format("RecNForget saved to {0}", LastFileName), applicationIcon, true);
					recordingTimer.Stop();
				});

			ToggleRecordButton.Focus();
		}

		private Icon getIcon()
		{
			var bmp = Bitmap.FromFile(logoPath);
			var thumb = (Bitmap)bmp.GetThumbnailImage(64, 64, null, IntPtr.Zero);
			thumb.MakeTransparent();
			return System.Drawing.Icon.FromHandle(thumb.GetHicon());
		}

		private void SwitchToBackgroundMode()
		{
			this.Hide();
			taskBarIcon.Visibility = Visibility.Visible;

			taskBarIcon.ShowBalloonTip("Running in background now!", @"RecNForget is now running in the background\ndouble click tray icon to restore", applicationIcon, true);
		}

		private void SwitchToForegroundMode()
		{
			this.Show();
			taskBarIcon.Visibility = Visibility.Collapsed;
		}

		#region configuration event handlers

		protected override void OnClosing(CancelEventArgs e)
		{
			if (MinimizedToTray)
			{
				var closeOrBackgroundDialog = new CloseOrBackgroundDialog();
				var dialogResult = closeOrBackgroundDialog.ShowDialog();

				if (dialogResult.HasValue && dialogResult.Value)
				{
					e.Cancel = true;
					SwitchToBackgroundMode();
				}
			}
		}

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

		private void RecordingTimer_Tick(object sender, EventArgs e)
		{
			RecordingTimeInMilliSeconds = TimeSpan.FromMilliseconds(DateTime.Now.Subtract(RecordingStart).TotalMilliseconds).TotalSeconds.ToString(@"0.##");
			UpdateCurrentFileNameDisplay();
		}

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

		private void UpdateCurrentFileNameDisplay()
		{
			if (CurrentlyRecording)
			{
				CurrentFileNameDisplay = hotkeyService == null ? string.Empty : string.Format("recording to {0} (for {1} s)", hotkeyService.CurrentFileName, RecordingTimeInMilliSeconds);
			}
			else
			{
				CurrentFileNameDisplay = string.Empty;
			}
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
			var settingsWindow = new SettingsWindow(hotkeyService, () => { SwitchToBackgroundMode(); }, () => { SwitchToForegroundMode(); });
			settingsWindow.ShowDialog();
		}

		#endregion
	}
}
