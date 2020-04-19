using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NAudio.Wave;
using RecNForget.Controls;
using RecNForget.IoC;
using RecNForget.Services.Contracts;
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
        private readonly IHotkeyService hotkeyService = null;

        // public ActionService(ISelectedFileService selectedFileService, IAudioPlaybackService audioPlaybackService, IAppSettingService appSettingService)
        public ActionService()
        {
            this.selectedFileService = UnityHandler.UnityContainer.Resolve<ISelectedFileService>();
            this.appSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
            this.audioPlaybackService = UnityHandler.UnityContainer.Resolve<IAudioPlaybackService>();
            this.audioRecordingService = UnityHandler.UnityContainer.Resolve<IAudioRecordingService>();
            this.hotkeyService = UnityHandler.UnityContainer.Resolve<IHotkeyService>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ChangeFileNamePattern()
        {
            throw new System.NotImplementedException();
        }

        public void ChangeOutputFolder()
        {
            throw new System.NotImplementedException();
        }

        public void ChangeSelectedFileName(DependencyObject ownerControl)
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

            if (!appSettingService.MinimizedToTray)
            {
                tempDialog.Owner = Window.GetWindow(ownerControl);
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

        public async void CheckForUpdates(DependencyObject ownerControl = null, bool showMessages = false)
        {
            try
            {
                var newerReleases = await UpdateChecker.GetNewerReleases(oldVersionString: ThisAssembly.AssemblyFileVersion);

                if (newerReleases.Any())
                {
                    string changeLog = UpdateChecker.GetAllChangeLogs(newerReleases);

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        var installUpdateDialog = new ReleaseInstallationDialog(newerReleases.First(), UpdateChecker.GetValidVersionStringMsiAsset(newerReleases.First()), changeLog);

                        if (ownerControl != null)
                        {
                            installUpdateDialog.Owner = Window.GetWindow(ownerControl);
                        }

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

                            if (ownerControl != null)
                            {
                                tempDialog.Owner = Window.GetWindow(ownerControl);
                            }

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

                        if (!appSettingService.MinimizedToTray)
                        {
                            errorDialog.Owner = Window.GetWindow(ownerControl);
                        }

                        errorDialog.ShowDialog();
                    });
                }
            }
        }

        public void DeleteSelectedFile(DependencyObject ownerControl)
        {
            audioPlaybackService.Stop();
            audioPlaybackService.KillAudio();

            CustomMessageBox tempDialog = new CustomMessageBox(
                caption: "Are you sure you want to delete this file?",
                icon: CustomMessageBoxIcon.Question,
                buttons: CustomMessageBoxButtons.OkAndCancel,
                messageRows: new List<string>() { selectedFileService.SelectedFile.FullName },
                controlFocus: CustomMessageBoxFocus.Ok);

            if (!appSettingService.MinimizedToTray)
            {
                tempDialog.Owner = Window.GetWindow(ownerControl);
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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { CheckForUpdates(ownerControl: null, showMessages: true); });
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            var helpmenu = new HelpWindow();

            if (!appSettingService.MinimizedToTray)
            {
                //helpmenu.Owner = this;
            }

            helpmenu.Show();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var aboutDialog = new AboutWindow(appSettingService);

            if (!appSettingService.MinimizedToTray)
            {
                // aboutDialog.Owner = this;
            }

            aboutDialog.ShowDialog();
        }

        private void ToggleMinimizedToTray(object sender, RoutedEventArgs e)
        {
            appSettingService.MinimizedToTray = !appSettingService.MinimizedToTray;
        }

        private void ToggleAlwaysOnTop(object sender, RoutedEventArgs e)
        {
            appSettingService.WindowAlwaysOnTop = !appSettingService.WindowAlwaysOnTop;
        }

        private void ToggleSelectedFileControlVisibility(object sender, RoutedEventArgs e)
        {
            appSettingService.SelectedFileControlVisible = !appSettingService.SelectedFileControlVisible;
        }

        private void ToggleOutputPathControlVisibility(object sender, RoutedEventArgs e)
        {
            appSettingService.OutputPathControlVisible = !appSettingService.OutputPathControlVisible;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(hotkeyService, appSettingService);

            if (!appSettingService.MinimizedToTray)
            {
                //settingsWindow.Owner = this;
            }

            settingsWindow.ShowDialog();
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
    }
}