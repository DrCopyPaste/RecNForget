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
using System.Windows.Threading;

namespace RecNForget
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged
	{
		private string logoPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Img", "logo.png");

		private string recordStartAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "startRec.wav");

		private string recordStopAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "stopRec.wav");

		private Icon applicationIcon;
		private TaskbarIcon taskBarIcon;

		private AudioPlayListService replayAudioService = null;
		private AudioPlayListService recordingFeedbackAudioService = null;

		private HotkeyService hotkeyService;
		private bool currentlyRecording = false;
		private bool currentlyNotRecording = true;
		private bool currentAudioPlayState = true;
		private string taskBar_ProgressState = "Paused";

		private DispatcherTimer recordingTimer;

		private string recordingTimeInMilliSeconds = string.Empty;
		private bool hasLastRecording = false;
		private string currentFileNameDisplay;
		private string lastFileName;
		private string lastFileNameDisplay;

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
				return currentFileNameDisplay;
			}

			set
			{
				currentFileNameDisplay = value;
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

		public string LastFileNameDisplay
		{
			get
			{
				return lastFileNameDisplay;
			}

			set
			{
				lastFileNameDisplay = value;

				if (!string.IsNullOrWhiteSpace(lastFileNameDisplay))
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

		public bool AutoReplayAudioAfterRecording
		{
			get
			{
				return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AutoReplayAudioAfterRecording"]);
			}
		}

		public bool PlayAudioFeedBackMarkingStartAndStopRecording
		{
			get
			{
				return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PlayAudioFeedBackMarkingStartAndStopRecording"]);
			}
		}

		public bool PlayAudioFeedBackMarkingStartAndStopReplaying
		{
			get
			{
				return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PlayAudioFeedBackMarkingStartAndStopReplaying"]);
			}
		}

		public string OutputPath
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings["OutputPath"];
			}
		}

		public bool ShowBalloonTipsForRecording
		{
			get
			{
				return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ShowBalloonTipsForRecording"]);
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
			taskBarIcon.Visibility = Visibility.Visible;

			replayAudioService = new AudioPlayListService(
				beforePlayAction: () =>
				{
					CurrentAudioPlayState = true;
					ReplayLastRecordingButton.Content = "Pause";
				}
				, afterStopAction: () =>
				{
					CurrentAudioPlayState = false;
					ReplayLastRecordingButton.Content = "Replay Last";
				}, afterPauseAction: () =>
				{
					CurrentAudioPlayState = false;
					ReplayLastRecordingButton.Content = "Resume";
				});

			recordingFeedbackAudioService = new AudioPlayListService(null, null, null);

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
					RecordingStart = DateTime.Now;
					recordingTimer.Start();
					UpdateLastFileName(reset: true);
					UpdateLastFileNameDisplay(reset: true);
					UpdateCurrentFileNameDisplay();
					if (ShowBalloonTipsForRecording)
					{
						taskBarIcon.ShowBalloonTip("Recording started!", "RecNForget now recording...", applicationIcon, true);
					}
					if (PlayAudioFeedBackMarkingStartAndStopRecording)
					{
						PlayRecordingStartAudioFeedback();
					}
				},
				stopRecordingAction: () =>
				{
					UpdateLastFileName();
					UpdateLastFileNameDisplay();
					UpdateCurrentFileNameDisplay(reset: true);
					CurrentlyRecording = false;
					CurrentlyNotRecording = true;
					ToggleRecordButton.Content = "Record";
					TaskBar_ProgressState = "Paused";
					if (ShowBalloonTipsForRecording)
					{
						taskBarIcon.ShowBalloonTip("Recording saved!", LastFileNameDisplay, applicationIcon, true);
					}
					recordingTimer.Stop();
					if (PlayAudioFeedBackMarkingStartAndStopRecording)
					{
						PlayRecordingStopAudioFeedback();
					}
					if (AutoReplayAudioAfterRecording)
					{
						ReplayLastRecording();
					}
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
			taskBarIcon.ShowBalloonTip("Running in background now!", @"RecNForget is now running in the background. Double click tray icon to restore", applicationIcon, true);
		}

		private void SwitchToForegroundMode()
		{
			this.Show();
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
				PlayRecordingStartAudioFeedback();
			}
		}

		private void UpdateCurrentFileNameDisplay(bool reset = false)
		{
			if (reset)
			{
				CurrentFileNameDisplay = string.Empty;
			}
			else
			{
				CurrentFileNameDisplay = hotkeyService == null ? string.Empty : string.Format("{0} ({1} s)", hotkeyService.CurrentFileName, RecordingTimeInMilliSeconds);
			}
			
		}

		private void UpdateLastFileName(bool reset = false)
		{
			lastFileName = reset ? string.Empty : hotkeyService == null ? string.Empty : hotkeyService.LastFileName;
		}

		private void UpdateLastFileNameDisplay(bool reset = false)
		{
			LastFileNameDisplay = reset ? string.Empty : string.Format("{0} ({1} s)", lastFileName, RecordingTimeInMilliSeconds);
		}

		private void OpenOutputFolder_Click(object sender, RoutedEventArgs e)
		{
			Process.Start(OutputPath);
		}

		private void ReplayLastRecording_Click(object sender, RoutedEventArgs e)
		{
			ReplayLastRecording();
		}

		// https://github.com/naudio/NAudio/blob/master/Docs/PlayAudioFileWinForms.md
		private void ReplayLastRecording()
		{
			if (replayAudioService.PlaybackState == PlaybackState.Stopped)
			{
				if (PlayAudioFeedBackMarkingStartAndStopReplaying)
				{
					replayAudioService.QueueFile(recordStartAudioFeedbackPath);
				}

				replayAudioService.QueueFile(lastFileName);

				if (PlayAudioFeedBackMarkingStartAndStopReplaying)
				{
					replayAudioService.QueueFile(recordStopAudioFeedbackPath);
				}

				replayAudioService.Play();
			}
			else if (replayAudioService.PlaybackState == PlaybackState.Playing)
			{
				replayAudioService.Pause();
			}
			else if (replayAudioService.PlaybackState == PlaybackState.Paused)
			{
				replayAudioService.Play();
			}
		}

		private void PlayRecordingStartAudioFeedback()
		{
			recordingFeedbackAudioService.QueueFile(recordStartAudioFeedbackPath);
			recordingFeedbackAudioService.Play();

			while (recordingFeedbackAudioService.PlaybackState == PlaybackState.Playing) { }
		}

		private void PlayRecordingStopAudioFeedback()
		{
			recordingFeedbackAudioService.QueueFile(recordStopAudioFeedbackPath);
			recordingFeedbackAudioService.Play();

			while (recordingFeedbackAudioService.PlaybackState == PlaybackState.Playing) { }
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			var settingsWindow = new SettingsWindow(hotkeyService, () => { SwitchToBackgroundMode(); }, () => { SwitchToForegroundMode(); });
			settingsWindow.ShowDialog();
		}

		#endregion
	}
}
