using NAudio.Wave;
using Notifications.Wpf.Core;
using Ookii.Dialogs.Wpf;
using RecNForget.Controls.Extensions;
using RecNForget.Controls.IoC;
using RecNForget.Help;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unity;

// ToDo remove coupling with controls from this service, so this service can be moved to RecNForget.Services (it is really only the custom message box so far)
namespace RecNForget.Controls.Services
{
    public class ActionService : IActionService
    {
        private readonly ISelectedFileService selectedFileService = null;
        private readonly IAudioPlaybackService audioPlaybackService = null;
        private readonly IAudioRecordingService audioRecordingService = null;
        private readonly IAppSettingService appSettingService = null;

        private readonly NotificationManager _notificationManager = new NotificationManager();
        public Control OwnerControl { get; set; }

        // public ActionService(ISelectedFileService selectedFileService, IAudioPlaybackService audioPlaybackService, IAppSettingService appSettingService)
        public ActionService()
        {
            this.selectedFileService = UnityHandler.UnityContainer.Resolve<ISelectedFileService>();
            this.appSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
            this.audioPlaybackService = UnityHandler.UnityContainer.Resolve<IAudioPlaybackService>();
            this.audioRecordingService = UnityHandler.UnityContainer.Resolve<IAudioRecordingService>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ChangeFileNamePattern()
        {
            CustomMessageBox tempDialog = new CustomMessageBox(
                caption: "Type in a new pattern for file name generation.",
                icon: CustomMessageBoxIcon.Question,
                buttons: CustomMessageBoxButtons.OkAndCancel,
                messageRows: new List<string>() { "Supported placeholders:", "(Date), (Guid)", "If you do not provide a placeholder to create unique file names, RecNForget will do it for you." },
                prompt: appSettingService.FilenamePrefix,
                controlFocus: CustomMessageBoxFocus.Prompt,
                promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

            tempDialog.TrySetViewablePositionFromOwner(OwnerControl);

            if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
            {
                appSettingService.FilenamePrefix = tempDialog.PromptContent;
            }
        }

        public void ChangeOutputFolder()
        {
            var dialog = new VistaFolderBrowserDialog();

            bool result =
                OwnerControl != null ?
                dialog.ShowDialog(Window.GetWindow(OwnerControl)) == true :
                dialog.ShowDialog() == true;

            if (result)
            {
                appSettingService.OutputPath = dialog.SelectedPath;
                selectedFileService.SelectLatestFile();
            }
        }

        public void ExportSelectedFile()
        {
            audioPlaybackService.Stop();
            audioPlaybackService.KillAudio();

            string preferredFileName = string.Empty;

            if (appSettingService.PromptForExportFileName)
            {
                CustomMessageBox tempDialog = new CustomMessageBox(
                    caption: "Select a filename for the exported file",
                    icon: CustomMessageBoxIcon.Question,
                    buttons: CustomMessageBoxButtons.OkAndCancel,
                    messageRows: new List<string>(),
                    prompt: Path.GetFileNameWithoutExtension(selectedFileService.SelectedFile.Name),
                    controlFocus: CustomMessageBoxFocus.Prompt,
                    promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

                tempDialog.TrySetViewablePositionFromOwner(OwnerControl);

                if (!tempDialog.ShowDialog().HasValue || !tempDialog.Ok)
                {
                    return;
                }

                preferredFileName = tempDialog.PromptContent;
            }

            var exportedFileName = selectedFileService.ExportFile(preferredFileName);
            if (string.IsNullOrEmpty(exportedFileName))
            {
                _notificationManager.ShowAsync(
                    content: new NotificationContent()
                    {
                        Title = "Something went wrong",
                        Message = "An unknown error occurred trying to export the selected file",
                        Type = NotificationType.Error
                    },
                    expirationTime: TimeSpan.FromSeconds(10));
                return;
            }


            _notificationManager.ShowAsync(
              content: new NotificationContent()
              {
                  Type = NotificationType.Success,
                  Title = $"{selectedFileService.SelectedFile.Name} exported to MP3!",
                  Message = $"Export was successful, file has been exported to {exportedFileName}."
              },
              onClick: () =>
              {
                  string argument = "/select, \"" + exportedFileName + "\"";
                  System.Diagnostics.Process.Start("explorer.exe", argument);
              });
        }

        public void ChangeSelectedFileName()
        {
            audioPlaybackService.Stop();
            audioPlaybackService.KillAudio();

            CustomMessageBox tempDialog = new CustomMessageBox(
                caption: "Rename the selected file",
                icon: CustomMessageBoxIcon.Question,
                buttons: CustomMessageBoxButtons.OkAndCancel,
                messageRows: new List<string>(),
                prompt: Path.GetFileNameWithoutExtension(selectedFileService.SelectedFile.Name),
                controlFocus: CustomMessageBoxFocus.Prompt,
                promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

            tempDialog.TrySetViewablePositionFromOwner(OwnerControl);

            if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
            {
                if (!selectedFileService.RenameSelectedFileWithoutExtension(tempDialog.PromptContent))
                {
                    _notificationManager.ShowAsync(
                        content: new NotificationContent()
                        {
                            Title = "Something went wrong",
                            Message = "An unknown error occurred trying to rename the selected file",
                            Type = NotificationType.Error
                        },
                        expirationTime: TimeSpan.FromSeconds(10));
                }
            }
        }

        public async void CheckForUpdates(bool showMessages = false)
        {
            try
            {
                var newerReleases = await UpdateChecker.GetNewerReleases(oldVersionString: appSettingService.RuntimeVersionString);

                if (newerReleases.Any())
                {
                    string changeLog = UpdateChecker.GetAllChangeLogs(newerReleases);

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        var installUpdateDialog = new ReleaseInstallationDialog(newerReleases.First(), UpdateChecker.GetValidVersionStringMsiAsset(newerReleases.First()), changeLog);

                        installUpdateDialog.TrySetViewablePositionFromOwner(OwnerControl);
                        installUpdateDialog.ShowDialog();
                    });
                }
                else
                {
                    if (showMessages)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            _notificationManager.ShowAsync(
                                content: new NotificationContent()
                                {
                                    Title = "No newer version found.",
                                    Message = "RecNForget is already up to date.",
                                    Type = NotificationType.Information
                                },
                                expirationTime: TimeSpan.FromSeconds(10));
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                if (showMessages)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        _notificationManager.ShowAsync(
                            content: new NotificationContent()
                            {
                                Title = "Error during update",
                                Message = "An error occurred trying to get updates:",
                                Type = NotificationType.Error
                            },
                            expirationTime: TimeSpan.FromSeconds(10));
                    });
                }
            }
        }

        public void DeleteSelectedFile()
        {
            audioPlaybackService.Stop();
            audioPlaybackService.KillAudio();

            CustomMessageBox tempDialog = new CustomMessageBox(
                caption: "Are you sure you want to delete this file?",
                icon: CustomMessageBoxIcon.Question,
                buttons: CustomMessageBoxButtons.OkAndCancel,
                messageRows: new List<string>() { selectedFileService.SelectedFile.FullName },
                controlFocus: CustomMessageBoxFocus.Ok);

            tempDialog.TrySetViewablePositionFromOwner(OwnerControl);

            if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
            {
                if (!selectedFileService.DeleteSelectedFile())
                {
                    _notificationManager.ShowAsync(
                        content: new NotificationContent()
                        {
                            Title = "Something went wrong",
                            Message = "An unknown error occurred trying to delete the selected file.",
                            Type = NotificationType.Error
                        },
                        expirationTime: TimeSpan.FromSeconds(10));
                }
            }
        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        public void OpenOutputFolderInExplorer()
        {
            var directory = new DirectoryInfo(appSettingService.OutputPath);

            if (selectedFileService.HasSelectedFile && selectedFileService.SelectedFile.Exists)
            {
                // if there is a result select it in an explorer window
                string argument = "/select, \"" + selectedFileService.SelectedFile.FullName + "\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
            else
            {
                if (!directory.Exists)
                {
                    directory.Create();
                }

                // otherwise just open output path in explorer
                Process.Start(appSettingService.OutputPath);
            }
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

        public void ShowApplicationMenu()
        {
            var myMenu = new ApplicationMenu();

        }

        public void ShowAboutWindow()
        {
            var aboutDialog = new AboutWindow();
            aboutDialog.TrySetViewablePositionFromOwner(OwnerControl);

            aboutDialog.ShowDialog();
        }

        public void ShowHelpWindow()
        {
            var helpmenu = new HelpWindow();
            helpmenu.TrySetViewablePositionFromOwner(OwnerControl);

            helpmenu.Show();
        }

        public void ShowSettingsMenu()
        {
            var settingsWindow = UnityHandler.UnityContainer.Resolve<SettingsWindow>();

            settingsWindow.TrySetViewablePositionFromOwner(OwnerControl);

            settingsWindow.ShowDialog();
        }

        #region menu events

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        private void CheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { CheckForUpdates(showMessages: true); });
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            ShowHelpWindow();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAboutWindow();
        }

        private void ToggleMinimizedToTray(object sender, RoutedEventArgs e)
        {
            ToggleMinimizedToTray();
        }

        private void ToggleAlwaysOnTop(object sender, RoutedEventArgs e)
        {
            ToggleAlwaysOnTop();
        }

        private void ToggleRecordingTimerControlVisibility(object sender, RoutedEventArgs e)
        {
            ToggleRecordingTimerControlVisibility();
        }

        private void ToggleSelectedFileControlVisibility(object sender, RoutedEventArgs e)
        {
            ToggleSelectedFileControlVisibility();
        }

        private void ToggleOutputPathControlVisibility(object sender, RoutedEventArgs e)
        {
            ToggleOutputPathControlVisibility();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowSettingsMenu();
        }

        public void ShowNewToApplicationWindow()
        {
            var dia = UnityHandler.UnityContainer.Resolve<NewToApplicationWindow>();

            if (!appSettingService.MinimizedToTray && OwnerControl != null)
            {
                dia.TrySetViewablePositionFromOwner(OwnerControl);
            }

            dia.Show();
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
                    var quickTip = new QuickTipDialog(appSettingService, randomTip);

                    quickTip.TrySetViewablePositionFromOwner(OwnerControl);
                    quickTip.Show();
                });
        }

        #endregion
    }
}