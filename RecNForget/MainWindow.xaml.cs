using Hardcodet.Wpf.TaskbarNotification;
using NAudio.Wave;
using Ookii.Dialogs.Wpf;
using RecNForget.Services;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using System.Windows.Threading;

namespace RecNForget
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged
	{
		private string recordStartAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "startRec.wav");
		private string recordStopAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "stopRec.wav");

        private string replayStartAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "playbackStart.wav");
        private string replayStopAudioFeedbackPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "playbackStop.wav");

		private AudioPlayListService replayAudioService = null;
		private AudioPlayListService recordingFeedbackAudioService = null;

        private AudioRecordingService audioRecordingService;
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
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.WindowAlwaysOnTop));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.WindowAlwaysOnTop, value.ToString());
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
				return AppSettingHelper.GetAppConfigSetting(AppSettingHelper.FilenamePrefix);
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.FilenamePrefix, value);
				OnPropertyChanged();
			}
		}

		public bool AutoReplayAudioAfterRecording
		{
            get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.AutoReplayAudioAfterRecording));
			}
		}

		public bool PlayAudioFeedBackMarkingStartAndStopRecording
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.PlayAudioFeedBackMarkingStartAndStopRecording));
			}
		}

		public bool PlayAudioFeedBackMarkingStartAndStopReplaying
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.PlayAudioFeedBackMarkingStartAndStopReplaying));
			}
		}

		public string OutputPath
		{
			get
			{
				return AppSettingHelper.GetAppConfigSetting(AppSettingHelper.OutputPath);
			}
		}

		public bool ShowBalloonTipsForRecording
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.ShowBalloonTipsForRecording));
			}
		}

		public bool MinimizedToTray
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.MinimizedToTray));
			}
		}

		#endregion

		public MainWindow()
		{
            // ensure AppConfig Values exist
            AppSettingHelper.RestoreDefaultAppConfigSetting(settingKey: null, overrideSetting: false);

            DataContext = this;
			InitializeComponent();

            taskBarIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            taskBarIcon.DoubleClickCommand = new RestoreMainWindowFromTrayCommand(() => { SwitchToForegroundMode(); });
			taskBarIcon.Visibility = Visibility.Visible;

            replayAudioService = new AudioPlayListService(
				beforePlayAction: () =>
				{
                    TaskBar_ProgressState = "Normal";
                    CurrentAudioPlayState = true;
                    StopReplayLastRecordingButton.IsEnabled = true;
                    ReplayLastRecordingStopDisabledIcon.Visibility = Visibility.Collapsed;
                    ReplayLastRecordingStopIcon.Visibility = Visibility.Visible;
                    ReplayLastRecordingPlayDisabledIcon.Visibility = Visibility.Collapsed;
                    ReplayLastRecordingPlayIcon.Visibility = Visibility.Collapsed;
                    ReplayLastRecordingPauseIcon.Visibility = Visibility.Visible;
                }, afterStopAction: () =>
				{
                    replayAudioService.KillAudio(reset: true);
                    CurrentAudioPlayState = false;
                    TaskBar_ProgressState = "Paused";
                    StopReplayLastRecordingButton.IsEnabled = false;
                    ReplayLastRecordingStopDisabledIcon.Visibility = Visibility.Visible;
                    ReplayLastRecordingStopIcon.Visibility = Visibility.Collapsed;

                    ReplayLastRecordingPlayDisabledIcon.Visibility = HasLastRecording ? Visibility.Collapsed : Visibility.Visible;
                    ReplayLastRecordingPlayIcon.Visibility = HasLastRecording ? Visibility.Visible : Visibility.Collapsed;
                    ReplayLastRecordingPauseIcon.Visibility = Visibility.Collapsed;
                }, afterPauseAction: () =>
                {
                    ReplayLastRecordingPlayDisabledIcon.Visibility = Visibility.Collapsed;
                    ReplayLastRecordingPlayIcon.Visibility = Visibility.Visible;
                    ReplayLastRecordingPauseIcon.Visibility = Visibility.Collapsed;
                }, afterResumeAction: () =>
                {
                    ReplayLastRecordingPlayDisabledIcon.Visibility = Visibility.Collapsed;
                    ReplayLastRecordingPlayIcon.Visibility = Visibility.Collapsed;
                    ReplayLastRecordingPauseIcon.Visibility = Visibility.Visible;
                });

			recordingFeedbackAudioService = new AudioPlayListService();

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

			this.Topmost = WindowAlwaysOnTop;

            this.audioRecordingService = new AudioRecordingService(
                startRecordingAction: () =>
                {
                    CurrentAudioPlayState = false;
                    ReplayLastRecordingButton.IsEnabled = false;
                    replayAudioService.KillAudio(reset: true);
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
                        taskBarIcon.ShowBalloonTip("Recording started!", "RecNForget now recording...", taskBarIcon.Icon, true);
                        taskBarIcon.TrayBalloonTipClicked -= TaskBarIcon_TrayBalloonTipClicked;
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
                        taskBarIcon.ShowBalloonTip("Recording saved!", LastFileNameDisplay, taskBarIcon.Icon, true);
                        taskBarIcon.TrayBalloonTipClicked += TaskBarIcon_TrayBalloonTipClicked;
                    }
                    recordingTimer.Stop();
                    if (PlayAudioFeedBackMarkingStartAndStopRecording)
                    {
                        PlayRecordingStopAudioFeedback();
                    }
                    ReplayLastRecordingButton.IsEnabled = true;
                    ReplayLastRecordingPlayDisabledIcon.Visibility = Visibility.Collapsed;
                    ReplayLastRecordingPlayIcon.Visibility = Visibility.Visible;
                    ReplayLastRecordingPauseIcon.Visibility = Visibility.Collapsed;
                    StopReplayLastRecordingButton.IsEnabled = true;
                    ReplayLastRecordingStopDisabledIcon.Visibility = Visibility.Collapsed;
                    ReplayLastRecordingStopIcon.Visibility = Visibility.Visible;
                    if (AutoReplayAudioAfterRecording)
                    {
                        ToggleReplayLastRecording();
                    }
                });

            this.hotkeyService = new HotkeyService();
            this.hotkeyService.AddHotkey(() => { return HotkeyToStringTranslator.GetHotkeySettingAsString(AppSettingHelper.HotKey_StartStopRecording); }, audioRecordingService.ToggleRecording);

            ToggleRecordButton.Focus();
		}

		private void TaskBarIcon_TrayBalloonTipClicked(object sender, RoutedEventArgs e)
		{
			if (!File.Exists(lastFileName))
			{
				return;
			}

			string argument = "/select, \"" + lastFileName + "\"";

			System.Diagnostics.Process.Start("explorer.exe", argument);
		}

		private void SwitchToBackgroundMode()
		{
			this.Hide();
			taskBarIcon.ShowBalloonTip("Running in background now!", @"RecNForget is now running in the background. Double click tray icon to restore", taskBarIcon.Icon, true);
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
			// https://stackoverflow.com/a/23182807
			FilenamePrefix = string.Concat(FilenamePrefix.Split(Path.GetInvalidFileNameChars()));

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
            audioRecordingService.ToggleRecording();
        }

		private void UpdateCurrentFileNameDisplay(bool reset = false)
		{
			if (reset)
			{
				CurrentFileNameDisplay = string.Empty;
			}
			else
			{
				CurrentFileNameDisplay = hotkeyService == null ? string.Empty : string.Format("{0} ({1} s)", audioRecordingService.CurrentFileName, RecordingTimeInMilliSeconds);
			}
			
		}

		private void UpdateLastFileName(bool reset = false)
		{
			lastFileName = reset ? string.Empty : hotkeyService == null ? string.Empty : audioRecordingService.LastFileName;
		}

		private void UpdateLastFileNameDisplay(bool reset = false)
		{
			LastFileNameDisplay = reset ? string.Empty : string.Format("{0} ({1} s)", audioRecordingService.LastFileName, RecordingTimeInMilliSeconds);
		}

		private void OpenOutputFolder_Click(object sender, RoutedEventArgs e)
		{
			Process.Start(OutputPath);
		}

        private void StopReplayLastRecording_Click(object sender, RoutedEventArgs e)
        {
            replayAudioService.Stop();
            replayAudioService.KillAudio(reset: true);
        }

        private void ReplayLastRecording_Click(object sender, RoutedEventArgs e)
		{
			ToggleReplayLastRecording();
		}

		// https://github.com/naudio/NAudio/blob/master/Docs/PlayAudioFileWinForms.md
		private void ToggleReplayLastRecording()
		{
			bool replayFileExists = false;

			if (replayAudioService.PlaybackState == PlaybackState.Stopped)
			{
				if (PlayAudioFeedBackMarkingStartAndStopReplaying)
				{
					replayAudioService.QueueFile(replayStartAudioFeedbackPath);
				}

				replayFileExists = replayAudioService.QueueFile(lastFileName);

				if (PlayAudioFeedBackMarkingStartAndStopReplaying)
				{
					replayAudioService.QueueFile(replayStopAudioFeedbackPath);
				}

				if (replayFileExists)
				{
					replayAudioService.Play();
				}
				else
				{
                    UpdateLastFileName(reset: true);
                    UpdateLastFileNameDisplay(reset: true);

                    HasLastRecording = false;

					CurrentAudioPlayState = false;
					ReplayLastRecordingButton.IsEnabled = false;
                    StopReplayLastRecordingButton.IsEnabled = false;

                    ReplayLastRecordingStopDisabledIcon.Visibility = Visibility.Visible;
                    ReplayLastRecordingStopIcon.Visibility = Visibility.Collapsed;

                    ReplayLastRecordingPlayDisabledIcon.Visibility = Visibility.Visible;
                    ReplayLastRecordingPlayIcon.Visibility = Visibility.Collapsed;
                    ReplayLastRecordingPauseIcon.Visibility = Visibility.Collapsed;

                    System.Windows.MessageBox.Show("The last recorded audio file has been moved or deleted.", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

			recordingFeedbackAudioService.KillAudio(reset: true);
		}

		private void PlayRecordingStopAudioFeedback()
		{
			recordingFeedbackAudioService.QueueFile(recordStopAudioFeedbackPath);
			recordingFeedbackAudioService.Play();

			while (recordingFeedbackAudioService.PlaybackState == PlaybackState.Playing) { }

			recordingFeedbackAudioService.KillAudio(reset: true);
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			var settingsWindow = new SettingsWindow(hotkeyService, () => { SwitchToBackgroundMode(); }, () => { SwitchToForegroundMode(); });
			settingsWindow.ShowDialog();
		}

		#endregion
	}
}
