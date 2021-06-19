using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NAudio.Wave;
using Notifications.Wpf.Core;
using Ookii.Dialogs.Wpf;
using RecNForget.Controls;
using RecNForget.Controls.Extensions;
using RecNForget.Controls.Helper;
using RecNForget.Controls.Services;
using RecNForget.Help;
using RecNForget.IoC;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;
using Unity;

// ToDo remove coupling with controls from this service, so this service can be moved to RecNForget.Services (it is really only the custom message box so far)
namespace RecNForget.WPF.Services
{
    public class ActionService : IActionService
    {
        private readonly ISelectedFileService selectedFileService = null;
        private readonly IAudioPlaybackService audioPlaybackService = null;
        private readonly IAudioRecordingService audioRecordingService = null;
        private readonly IAppSettingService appSettingService = null;
        private readonly IApplicationHotkeyService hotkeyService = null;

        private readonly NotificationManager _notificationManager = new NotificationManager();

        public Control OwnerControl { get; set; }

        // public ActionService(ISelectedFileService selectedFileService, IAudioPlaybackService audioPlaybackService, IAppSettingService appSettingService)
        public ActionService()
        {
            this.selectedFileService = UnityHandler.UnityContainer.Resolve<ISelectedFileService>();
            this.appSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
            this.audioPlaybackService = UnityHandler.UnityContainer.Resolve<IAudioPlaybackService>();
            this.audioRecordingService = UnityHandler.UnityContainer.Resolve<IAudioRecordingService>();
            this.hotkeyService = UnityHandler.UnityContainer.Resolve<IApplicationHotkeyService>();
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
                            CustomMessageBox tempDialog = new CustomMessageBox(
                                caption: "RecNForget is already up to date!",
                                icon: CustomMessageBoxIcon.Information,
                                buttons: CustomMessageBoxButtons.OK,
                                messageRows: new List<string>() { "No newer version found" },
                                controlFocus: CustomMessageBoxFocus.Ok);

                            tempDialog.TrySetViewablePositionFromOwner(OwnerControl);

                            tempDialog.ShowDialog();
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
                        var errorDialog = new CustomMessageBox(
                            caption: "Error during update",
                            icon: CustomMessageBoxIcon.Error,
                            buttons: CustomMessageBoxButtons.OK,
                            messageRows: new List<string>() { "An error occurred trying to get updates:", ex.InnerException.Message },
                            controlFocus: CustomMessageBoxFocus.Ok);

                        errorDialog.TrySetViewablePositionFromOwner(OwnerControl);

                        errorDialog.ShowDialog();
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
                    CustomMessageBox errorMessageBox = new CustomMessageBox(
                        caption: "Something went wrong",
                        icon: CustomMessageBoxIcon.Error,
                        buttons: CustomMessageBoxButtons.OK,
                        messageRows: new List<string>() { "An error occurred trying to delete the selected file" },
                        controlFocus: CustomMessageBoxFocus.Ok);

                    errorMessageBox.TrySetViewablePositionFromOwner(OwnerControl);

                    errorMessageBox.ShowDialog();
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
            var menu = new System.Windows.Controls.ContextMenu();
            menu.Style = (Style)menu.FindResource("ContextMenu_Default_Style");

            var item = new System.Windows.Controls.MenuItem()
            {
                Header = "Cancel",
                Style = (Style)menu.FindResource("ContextMenu_Cancel_Style")
            };
            menu.Items.Add(item);

            menu.Items.Add(new System.Windows.Controls.Separator() { Style = (Style)menu.FindResource("MenuSeparator_Style") });

            item = new System.Windows.Controls.MenuItem()
            {
                Header = "Open settings",
                Style = (Style)menu.FindResource("ContextMenu_Settings_Style"),
            };
            item.Click += SettingsButton_Click;
            menu.Items.Add(item);

            menu.Items.Add(new System.Windows.Controls.Separator() { Style = (Style)menu.FindResource("MenuSeparator_Style") });

            item = new System.Windows.Controls.MenuItem()
            {
                IsCheckable = true,
                IsChecked = appSettingService.OutputPathControlVisible,
                Header = "Output Path Control",
                Style = (Style)menu.FindResource("Base_ContextMenu_MenuItem_Style"),
            };
            item.Click += ToggleOutputPathControlVisibility;
            menu.Items.Add(item);

            item = new System.Windows.Controls.MenuItem()
            {
                IsCheckable = true,
                IsChecked = appSettingService.SelectedFileControlVisible,
                Header = "Selected File Control",
                Style = (Style)menu.FindResource("Base_ContextMenu_MenuItem_Style"),
            };
            item.Click += ToggleSelectedFileControlVisibility;
            menu.Items.Add(item);

            menu.Items.Add(new System.Windows.Controls.Separator() { Style = (Style)menu.FindResource("MenuSeparator_Style") });

            item = new System.Windows.Controls.MenuItem()
            {
                IsCheckable = true,
                IsChecked = appSettingService.WindowAlwaysOnTop,
                Header = "always on top",
                Style = (Style)menu.FindResource("Base_ContextMenu_MenuItem_Style"),
            };
            item.Click += ToggleAlwaysOnTop;
            menu.Items.Add(item);

            item = new System.Windows.Controls.MenuItem()
            {
                IsCheckable = true,
                IsChecked = appSettingService.MinimizedToTray,
                Header = "run in background",
                Style = (Style)menu.FindResource("Base_ContextMenu_MenuItem_Style"),
            };
            item.Click += ToggleMinimizedToTray;
            menu.Items.Add(item);

            menu.Items.Add(new System.Windows.Controls.Separator() { Style = (Style)menu.FindResource("MenuSeparator_Style") });

            item = new System.Windows.Controls.MenuItem()
            {
                Header = "About RecNForget",
                Style = (Style)menu.FindResource("ContextMenu_About_Style"),
            };
            item.Click += AboutButton_Click;
            menu.Items.Add(item);

            item = new System.Windows.Controls.MenuItem()
            {
                Header = "Help",
                Style = (Style)menu.FindResource("ContextMenu_Help_Style"),
            };
            item.Click += Help_Click;
            menu.Items.Add(item);

            menu.Items.Add(new System.Windows.Controls.Separator() { Style = (Style)menu.FindResource("MenuSeparator_Style") });

            item = new System.Windows.Controls.MenuItem()
            {
                Header = "Check for updates",
                Style = (Style)menu.FindResource("ContextMenu_CheckUpdates_Style"),
            };
            item.Click += CheckUpdates_Click;
            menu.Items.Add(item);

            menu.Items.Add(new System.Windows.Controls.Separator() { Style = (Style)menu.FindResource("MenuSeparator_Style") });

            item = new System.Windows.Controls.MenuItem()
            {
                Header = "Close RecNForget",
                Style = (Style)menu.FindResource("ContextMenu_ShutDown_Style"),
            };
            item.Click += Exit_Click;
            menu.Items.Add(item);

            menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
            menu.IsOpen = true;
        }

        public void ShowThemeSelectionMenu()
        {
            var menu = new System.Windows.Controls.ContextMenu();
            menu.Style = (Style)menu.FindResource("ContextMenu_Default_Style");

            //var item = new System.Windows.Controls.MenuItem()
            //{
            //    Header = "Cancel",
            //    Style = (Style)menu.FindResource("ContextMenu_Cancel_Style")
            //};
            //menu.Items.Add(item);

            //menu.Items.Add(new System.Windows.Controls.Separator() { Style = (Style)menu.FindResource("MenuSeparator_Style") });

            foreach (var themeName in ThemeManager.GetAllThemeNames())
            {
                var item = new System.Windows.Controls.MenuItem()
                {
                    Header = themeName,
                    Style = (Style)menu.FindResource("Base_ContextMenu_MenuItem_Style"),
                    IsCheckable = true,
                    IsChecked = appSettingService.WindowTheme.ToUpper() == themeName.ToUpper()
                };
                item.Click += (object sender, RoutedEventArgs e) => { ThemeManager.ChangeTheme(themeName); appSettingService.WindowTheme = themeName; };

                menu.Items.Add(item);
            }

            menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
            menu.IsOpen = true;
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
            var settingsWindow = new SettingsWindow(hotkeyService, appSettingService);
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
            var dia = new NewToApplicationWindow(hotkeyService, appSettingService, this);

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