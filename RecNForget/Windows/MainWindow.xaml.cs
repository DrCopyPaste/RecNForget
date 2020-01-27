﻿using Hardcodet.Wpf.TaskbarNotification;
using NAudio.Wave;
using Ookii.Dialogs.Wpf;
using RecNForget.Controls;
using RecNForget.Services;
using RecNForget.Services.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using System.Windows.Threading;

namespace RecNForget.Windows
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

		private string recordingTimeimeDisplay = string.Empty;
		private string currentFileNameDisplay;

		SelectedFileService selectedFileService;
		public SelectedFileService SelectedFileService
		{
			get
			{
				return selectedFileService;
			}
			set
			{
				selectedFileService = value;
				OnPropertyChanged();
			}
		}
		public DateTime RecordingStart { get; set; }

        #region bound values

        public string RecordingTimeDisplay
		{
			get
			{
				return CurrentlyRecording ? recordingTimeimeDisplay : string.Empty;
			}
			set
			{
                recordingTimeimeDisplay = value;
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

                UpdateCurrentFileNameDisplay();
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

				UpdateCurrentFileNameDisplay();
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

		public bool AutoSelectLastRecording
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.AutoSelectLastRecording));
			}
		}

		public bool CheckForUpdateOnStart
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.CheckForUpdateOnStart));
			}
		}

		public bool ShowTipsOnStart
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.ShowTipsAtApplicationStart));
			}
		}

		#endregion

		private void ShowRandomApplicationTip()
		{
			var allFeatures = Services.Types.HelpFeature.All;

			int randomNumber = (new Random()).Next(0, allFeatures.Count - 1);
			var randomFeature = allFeatures[randomNumber];
			List<string> displayLines = new List<string>();
			foreach (var line in randomFeature.HelpLines)
			{
				displayLines.Add(line.Content);
			}

			CustomMessageBox tempDialog = new CustomMessageBox(
				caption: "Did you know?",
				icon: CustomMessageBoxIcon.Information,
				buttons: CustomMessageBoxButtons.OK,
				messageRows: displayLines,
				controlFocus: CustomMessageBoxFocus.Ok);

			if (!MinimizedToTray)
			{
				tempDialog.Owner = this;
			}

			tempDialog.ShowDialog();
		}

		public MainWindow()
		{
			this.KeyDown += Window_KeyDown;
			this.MouseDown += Window_Click;

			// ensure AppConfig Values exist
			bool firstTimeUser = AppSettingHelper.RestoreDefaultAppConfigSetting(settingKey: null, overrideSetting: false);

			if (CheckForUpdateOnStart)
			{
				UpdateChecker.ShowUpdateDialogIfPossible(suppressUpToDateDialog: true);
			}

			replayAudioService = new AudioPlayListService(
				beforePlayAction: () =>
				{
                    TaskBar_ProgressState = "Normal";
                    CurrentAudioPlayState = true;
					PauseReplayLastRecordingButton.IsEnabled = true;
					ReplayLastRecordingButton.Visibility = Visibility.Collapsed;
					PauseReplayLastRecordingButton.Visibility = Visibility.Visible;
				}, afterStopAction: () =>
				{
                    replayAudioService.KillAudio(reset: true);
                    CurrentAudioPlayState = false;
                    TaskBar_ProgressState = "Paused";

					ReplayLastRecordingButton.IsEnabled = SelectedFileService.HasSelectedFile && !CurrentlyRecording;
					ReplayLastRecordingButton.Visibility = Visibility.Visible;
					PauseReplayLastRecordingButton.Visibility = Visibility.Collapsed;
				}, afterPauseAction: () =>
                {
					ReplayLastRecordingButton.IsEnabled = true;
					ReplayLastRecordingButton.Visibility = Visibility.Visible;
					PauseReplayLastRecordingButton.Visibility = Visibility.Collapsed;
				}, afterResumeAction: () =>
                {
					PauseReplayLastRecordingButton.IsEnabled = true;
					ReplayLastRecordingButton.Visibility = Visibility.Collapsed;
					PauseReplayLastRecordingButton.Visibility = Visibility.Visible;
				});

			recordingFeedbackAudioService = new AudioPlayListService();

			recordingTimer = new DispatcherTimer();
			recordingTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
			recordingTimer.Tick += new EventHandler(RecordingTimer_Tick);

            this.audioRecordingService = new AudioRecordingService(
                startRecordingAction: () =>
                {
					RecordButton.Visibility = Visibility.Collapsed;
					StopRecordButton.Visibility = Visibility.Visible;
					StopRecordButton.Focus();
					CurrentAudioPlayState = false;
                    ReplayLastRecordingButton.IsEnabled = false;
                    replayAudioService.KillAudio(reset: true);
                    CurrentlyRecording = true;
                    CurrentlyNotRecording = false;
                    TaskBar_ProgressState = "Error";
                    RecordingStart = DateTime.Now;
                    recordingTimer.Start();
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
					this.Focus();
					UpdateCurrentFileNameDisplay(reset: true);
                    CurrentlyRecording = false;
                    CurrentlyNotRecording = true;
                    TaskBar_ProgressState = "Paused";
                    if (ShowBalloonTipsForRecording)
                    {
                        taskBarIcon.ShowBalloonTip("Recording saved!", audioRecordingService.LastFileName, taskBarIcon.Icon, true);
                        taskBarIcon.TrayBalloonTipClicked += TaskBarIcon_TrayBalloonTipClicked;
                    }
                    recordingTimer.Stop();
                    if (PlayAudioFeedBackMarkingStartAndStopRecording)
                    {
                        PlayRecordingStopAudioFeedback();
                    }
					if (AutoSelectLastRecording)
					{
						SelectedFileService.SelectFile(new FileInfo(audioRecordingService.LastFileName));
					}
					ReplayLastRecordingButton.IsEnabled = true;
                    StopReplayLastRecordingButton.IsEnabled = true;
					RecordButton.Visibility = Visibility.Visible;
					StopRecordButton.Visibility = Visibility.Collapsed;
					if (AutoReplayAudioAfterRecording)
                    {
                        ToggleReplayLastRecording(audioRecordingService.LastFileName);
                    }
                },
                outputPathGetterMethod: () =>
                {
                    return AppSettingHelper.GetAppConfigSetting(AppSettingHelper.OutputPath);
                },
                filenamePrefixGetterMethod: () =>
                {
                    return AppSettingHelper.GetAppConfigSetting(AppSettingHelper.FilenamePrefix);
                });

            this.hotkeyService = new HotkeyService();
            this.hotkeyService.AddHotkey(() => { return HotkeyToStringTranslator.GetHotkeySettingAsString(AppSettingHelper.HotKey_StartStopRecording); }, audioRecordingService.ToggleRecording);

            DataContext = this;
            InitializeComponent();

			SelectedFileService = new SelectedFileService();
			if (SelectedFileService.SelectLatestFile())
			{
				ReplayLastRecordingButton.IsEnabled = true;
			}

			CurrentAudioPlayState = false;

			this.Topmost = WindowAlwaysOnTop;
            taskBarIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            taskBarIcon.DoubleClickCommand = new SimpleActionCommand(() => { SwitchToForegroundMode(); });
            taskBarIcon.Visibility = Visibility.Visible;

			UpdateCurrentFileNameDisplay(reset: true);

			if (MinimizedToTray)
			{
				SwitchToBackgroundMode();
			}
			else
			{
				SwitchToForegroundMode();
			}

			if (firstTimeUser)
			{
				var dia = new NewToApplicationWindow(this.hotkeyService)
				{
					Owner = this
				};

				dia.Show();
			}
			else if(ShowTipsOnStart)
			{
				ShowRandomApplicationTip();
			}
		}

		private void TaskBarIcon_TrayBalloonTipClicked(object sender, RoutedEventArgs e)
		{
			if (audioRecordingService.LastFileName == string.Empty || !File.Exists(audioRecordingService.LastFileName))
			{
				return;
			}

			string argument = "/select, \"" + audioRecordingService.LastFileName + "\"";
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
				var closeOrBackgroundDialog = new CloseOrBackgroundDialog()
				{
					Owner = this
				};

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

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				ChangeSelectedFileNameButton.PerformClick();
			}
			else if (e.Key == Key.Delete)
			{
				DeleteSelectedFileButton.PerformClick();
			}
			else if (e.Key == Key.Down)
			{
				OpenSelectedFileButton.PerformClick();
			}
			else if (e.Key == Key.Left)
			{
				SkipPrevButton.PerformClick();
			}
			else if (e.Key == Key.Right)
			{
				SkipNextButton.PerformClick();
			}
			else if (e.Key == Key.Space)
			{
				ToggleReplayLastRecording();
			}
			else if (e.Key == Key.Escape)
			{
				StopReplayLastRecordingButton.PerformClick();
			}
		}

		private void RecordingTimer_Tick(object sender, EventArgs e)
		{
			RecordingTimeDisplay = TimeSpan.FromMilliseconds(DateTime.Now.Subtract(RecordingStart).TotalMilliseconds).TotalSeconds.ToString(@"0.##");
			UpdateCurrentFileNameDisplay();
		}

		private void RecordButton_Click(object sender, RoutedEventArgs e)
		{
            audioRecordingService.ToggleRecording();
        }

		private void UpdateCurrentFileNameDisplay(bool reset = false)
		{
			CurrentFileNameDisplay = !CurrentlyRecording || reset || hotkeyService == null ?
                audioRecordingService.GetTargetPathTemplateString() :
                string.Format("{0} ({1} s)", audioRecordingService.CurrentFileName, RecordingTimeDisplay);
		}

		private void OpenOutputFolder_Click(object sender, RoutedEventArgs e)
		{
			var directory = new DirectoryInfo(OutputPath);

			if (SelectedFileService.HasSelectedFile && SelectedFileService.SelectedFile.Exists)
            {
				// if there is a result select it in an explorer window
				string argument = "/select, \"" + SelectedFileService.SelectedFile.FullName + "\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
            else if (directory.Exists)
			{
				// otherwise just open output path in explorer
				Process.Start(OutputPath);
            }
			else
			{
				// unselect selected file (it does not exist anymore)
				// disable skip buttons
				// disable open folder button?
				// show message box
			}
        }

        private void StopReplayLastRecording_Click(object sender, RoutedEventArgs e)
        {
            replayAudioService.Stop();
            replayAudioService.KillAudio(reset: true);
        }

		private void SkipPrevButton_Click(object sender, RoutedEventArgs e)
		{
			replayAudioService.Stop();
			replayAudioService.KillAudio(reset: true);
			SelectedFileService.SelectPrevFile();
		}

		private void SkipNextButton_Click(object sender, RoutedEventArgs e)
		{
			replayAudioService.Stop();
			replayAudioService.KillAudio(reset: true);
			SelectedFileService.SelectNextFile();
		}

		private void ReplayLastRecording_Click(object sender, RoutedEventArgs e)
		{
			ToggleReplayLastRecording();
		}

		// https://github.com/naudio/NAudio/blob/master/Docs/PlayAudioFileWinForms.md
		private void ToggleReplayLastRecording(string fileName = null)
		{
			bool replayFileExists = SelectedFileService.SelectedFile.Exists;

			if (replayAudioService.PlaybackState == PlaybackState.Stopped)
			{
				if (PlayAudioFeedBackMarkingStartAndStopReplaying)
				{
					replayAudioService.QueueFile(replayStartAudioFeedbackPath);
				}

				replayAudioService.QueueFile(fileName != null ? fileName : SelectedFileService.SelectedFile.FullName);

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
					SelectedFileService.SelectFile(null);
					CurrentAudioPlayState = false;
					ReplayLastRecordingButton.IsEnabled = false;
                    StopReplayLastRecordingButton.IsEnabled = false;

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

		private void Help_Click(object sender, RoutedEventArgs e)
		{
			var helpmenu = new HelpWindow();

			if (!MinimizedToTray)
			{
				helpmenu.Owner = this;
			}

			helpmenu.Show();
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			var settingsWindow = new SettingsWindow(hotkeyService, () => { SwitchToBackgroundMode(); }, () => { SwitchToForegroundMode(); });

			if (!MinimizedToTray)
			{
				settingsWindow.Owner = this;
			}

			settingsWindow.ShowDialog();

			UpdateCurrentFileNameDisplay();
		}

		private void AboutButton_Click(object sender, RoutedEventArgs e)
		{
			var aboutDialog = new AboutDialog();

			if (!MinimizedToTray)
			{
				aboutDialog.Owner = this;
			}

			aboutDialog.ShowDialog();
		}

		private void CheckUpdates_Click(object sender, RoutedEventArgs e)
		{
			UpdateChecker.ShowUpdateDialogIfPossible();
		}

		private void Window_Click(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Right)
			{
				ContextMenu contextMenu = WindowOptionsButton.ContextMenu;
				contextMenu.IsOpen = true;
			}
		}

		private void WindowOptionsButton_Click(object sender, RoutedEventArgs e)
        {
			Button button = sender as Button;
			ContextMenu contextMenu = button.ContextMenu;
			contextMenu.PlacementTarget = button;
			contextMenu.IsOpen = true;

			e.Handled = true;
		}

		private void ChangeFileNamePatternButton_Clicked(object sender, RoutedEventArgs e)
		{
			CustomMessageBox tempDialog = new CustomMessageBox(
				caption: "Type in a new pattern for file name generation.",
				icon: CustomMessageBoxIcon.Question,
				buttons: CustomMessageBoxButtons.OkAndCancel,
				messageRows: new List<string>() { "Supported placeholders:", "(Date), (Guid)", "If you do not provide a placeholder to create unique file names, RecNForget will do it for you." },
				prompt: AppSettingHelper.GetAppConfigSetting(AppSettingHelper.FilenamePrefix),
				controlFocus: CustomMessageBoxFocus.Prompt,
				promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

			if (!MinimizedToTray)
			{
				tempDialog.Owner = this;
			}

			if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
			{
				FilenamePrefix = tempDialog.PromptContent;
			}
		}

		private void ChangeOutputFolderButton_Clicked(object sender, RoutedEventArgs e)
		{
			var dialog = new VistaFolderBrowserDialog();

			if (dialog.ShowDialog(this) == true)
			{
				OutputPath = dialog.SelectedPath;
			}
		}

		private void ChangeSelectedFileNameButton_Clicked(object sender, RoutedEventArgs e)
		{
			replayAudioService.Stop();
			replayAudioService.KillAudio();

			CustomMessageBox tempDialog = new CustomMessageBox(
				caption: "Rename the selected file",
				icon: CustomMessageBoxIcon.Question,
				buttons: CustomMessageBoxButtons.OkAndCancel,
				messageRows: new List<string>(),
				prompt: Path.GetFileNameWithoutExtension(selectedFileService.SelectedFile.Name),
				controlFocus: CustomMessageBoxFocus.Prompt,
				promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

			if (!MinimizedToTray)
			{
				tempDialog.Owner = this;
			}

			if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
			{
				if (!selectedFileService.RenameSelectedFileWithoutExtension(tempDialog.PromptContent))
				{
					CustomMessageBox errorMessageBox = new CustomMessageBox(
						caption: "Something went wrong",
						icon: CustomMessageBoxIcon.Error,
						buttons: CustomMessageBoxButtons.OK,
						messageRows: new List<string>() { "An error occurred trying to rename the selected file" },
						controlFocus: CustomMessageBoxFocus.Ok);

					errorMessageBox.ShowDialog();
				}
			}
		}

		private void DeleteSelectedFileButton_Clicked(object sender, RoutedEventArgs e)
		{
			replayAudioService.Stop();
			replayAudioService.KillAudio();

			CustomMessageBox tempDialog = new CustomMessageBox(
				caption: "Are you sure you want to delete this file?",
				icon: CustomMessageBoxIcon.Question,
				buttons: CustomMessageBoxButtons.OkAndCancel,
				messageRows: new List<string>() { selectedFileService.SelectedFile.FullName },
				controlFocus: CustomMessageBoxFocus.Ok);

			if (!MinimizedToTray)
			{
				tempDialog.Owner = this;
			}

			if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
			{
				if (!selectedFileService.DeleteSelectedFile())
				{
					CustomMessageBox errorMessageBox = new CustomMessageBox(
						caption: "Something went wrong",
						icon: CustomMessageBoxIcon.Error,
						buttons: CustomMessageBoxButtons.OK,
						messageRows: new List<string>() { "An error occurred trying to delete the selected file" },
						controlFocus: CustomMessageBoxFocus.Ok);

					errorMessageBox.ShowDialog();
				}
			}
		}

		#endregion
	}
}