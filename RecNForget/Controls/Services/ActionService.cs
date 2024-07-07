using Microsoft.Extensions.DependencyInjection;
using NAudio.Wave;
using Notifications.Wpf.Core;
using RecNForget.Controls.Extensions;
using RecNForget.Controls.IoC;
using RecNForget.Help;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls.Services
{
    public class ActionService : IActionService
    {
        private readonly ISelectedFileService selectedFileService = null;
        private readonly IAudioPlaybackService audioPlaybackService = null;
        private readonly IAudioRecordingService audioRecordingService = null;
        private readonly IAppSettingService appSettingService = null;

        private readonly NotificationManager _notificationManager = new NotificationManager();
        private static bool checkingForUpdates = false;
        public Control OwnerControl { get; set; }

        // public ActionService(ISelectedFileService selectedFileService, IAudioPlaybackService audioPlaybackService, IAppSettingService appSettingService)
        public ActionService
        (
            ISelectedFileService selectedFileService,
            IAudioPlaybackService audioPlaybackService,
            IAudioRecordingService audioRecordingService,
            IAppSettingService appSettingService
        )
        {
            this.selectedFileService = selectedFileService;
            this.appSettingService = appSettingService;
            this.audioPlaybackService = audioPlaybackService;
            this.audioRecordingService = audioRecordingService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        public void SelectNextFile()
        {
            audioPlaybackService.Stop();
            selectedFileService.SelectNextFile();
        }

        public void SelectPreviousFile()
        {
            audioPlaybackService.Stop();
            selectedFileService.SelectPrevFile();
        }

        public void StopPlayingSelectedFile()
        {
            audioPlaybackService.Stop();
        }

        private void ToggleAlwaysOnTop()
        {
            appSettingService.WindowAlwaysOnTop = !appSettingService.WindowAlwaysOnTop;
        }

        private void ToggleMinimizedToTray()
        {
            appSettingService.MinimizedToTray = !appSettingService.MinimizedToTray;
        }

        public void TogglePlayPauseSelectedFile()
        {
            if (selectedFileService.HasSelectedFile && audioRecordingService.CurrentlyNotRecording)
            {
                if (audioPlaybackService.ItemsCount == 0)
                {
                    QueueAudioPlayback(
                        fileName: selectedFileService.SelectedFile.FullName,
                        startIndicatorFileName: appSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? audioPlaybackService.ReplayStartAudioFeedbackPath : null,
                        endIndicatorFileName: appSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? audioPlaybackService.ReplayStopAudioFeedbackPath : null);
                }

                TogglePlayPauseAudio();
            }
        }

        public void ToggleOutputPathControlVisibility()
        {
            appSettingService.OutputPathControlVisible = !appSettingService.OutputPathControlVisible;
        }

        public void ToggleRecordingTimerControlVisibility()
        {
            appSettingService.RecordingTimerControlVisible = !appSettingService.RecordingTimerControlVisible;
        }

        public void ToggleSelectedFileControlVisibility()
        {
            appSettingService.SelectedFileControlVisible = !appSettingService.SelectedFileControlVisible;
        }

        public void TogglePlayPauseAudio()
        {
            if (audioPlaybackService.PlaybackState == PlaybackState.Stopped)
            {
                audioPlaybackService.Play();
            }
            else if (audioPlaybackService.PlaybackState == PlaybackState.Playing)
            {
                audioPlaybackService.Pause();
            }
            else if (audioPlaybackService.PlaybackState == PlaybackState.Paused)
            {
                audioPlaybackService.Play();
            }
        }

        public void ToggleStartStopRecording()
        {
            audioRecordingService.ToggleRecording();
        }

        public bool QueueAudioPlayback(string fileName = null, string startIndicatorFileName = null, string endIndicatorFileName = null)
        {
            bool replayFileExists = false;
            string fileNameToPlay;

            if (fileName == null)
            {
                replayFileExists = selectedFileService.SelectedFile.Exists;
                fileNameToPlay = selectedFileService.SelectedFile.FullName;
            }
            else
            {
                var fileInfo = new FileInfo(fileName);
                replayFileExists = fileInfo.Exists;
                fileNameToPlay = fileName;
            }

            if (!replayFileExists)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(startIndicatorFileName))
            {
                audioPlaybackService.QueueFile(startIndicatorFileName);
            }

            audioPlaybackService.QueueFile(fileNameToPlay);

            if (!string.IsNullOrEmpty(endIndicatorFileName))
            {
                audioPlaybackService.QueueFile(endIndicatorFileName);
            }

            return true;
        }

        public void ShowAboutWindow()
        {
            var aboutDialog = ConfiguredServices.ServiceProvider.GetRequiredService<AboutWindow>();
            aboutDialog.TrySetViewablePositionFromOwner(OwnerControl);

            aboutDialog.ShowDialog();
        }

        public void ShowHelpWindow()
        {
            var helpmenu = ConfiguredServices.ServiceProvider.GetRequiredService<HelpWindow>();
            helpmenu.TrySetViewablePositionFromOwner(OwnerControl);

            helpmenu.Show();
        }

        public void ShowSettingsMenu()
        {
            var settingsWindow = ConfiguredServices.ServiceProvider.GetRequiredService<SettingsWindow>();

            settingsWindow.TrySetViewablePositionFromOwner(OwnerControl);

            settingsWindow.ShowDialog();
        }

        #region menu events

        public void ShowNewToApplicationWindow()
        {
            var dia = ConfiguredServices.ServiceProvider.GetRequiredService<NewToApplicationWindow>();

            if (!appSettingService.MinimizedToTray && OwnerControl != null)
            {
                dia.TrySetViewablePositionFromOwner(OwnerControl);
            }

            dia.ShowDialog();
        }

        public void ShowNewToVersionDialog(Version currentFileVersion, Version lastInstalledVersion)
        {
            var newToVersionDialog = new NewToVersionDialog(lastInstalledVersion, currentFileVersion, appSettingService);

            if (!appSettingService.MinimizedToTray && OwnerControl != null)
            {
                newToVersionDialog.TrySetViewablePositionFromOwner(OwnerControl);
            }

            newToVersionDialog.Show();
        }

        public void ShowRandomApplicationTip()
        {
            var randomTip = HelpFeature.GetRandomFeature();

            int rowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Did you know?");
            sb.AppendLine();

            foreach (var line in randomTip.HelpLines)
            {
                if (rowCount > 3) break;

                sb.AppendLine(line.Content);
                rowCount++;
            }

            if (rowCount < randomTip.HelpLines.Count)
            {
                sb.AppendLine();
                sb.AppendLine("... (click to read more)");
            }

            _notificationManager.ShowAsync(
                content: new NotificationContent()
                {
                    Title = randomTip.Title,
                    Message = sb.ToString(),
                    Type = NotificationType.Information
                },
                expirationTime: TimeSpan.FromSeconds(10),
                onClick: () =>
                {
                    var quickTip = ConfiguredServices.ServiceProvider.GetRequiredService<QuickTipDialog>();
                    quickTip.SetQuickTip(randomTip);
                    quickTip.TrySetViewablePositionFromOwner(OwnerControl);
                    quickTip.Show();
                });
        }

        #endregion
    }
}