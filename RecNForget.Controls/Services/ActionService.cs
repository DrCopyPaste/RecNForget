using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using NAudio.Wave;
using RecNForget.Controls;
using RecNForget.IoC;
using RecNForget.Services.Contracts;
using Unity;

// ToDo remove coupling with controls from this service, so this service can be moved to RecNForget.Services (it is really only the custom message box so far)
namespace RecNForget.Control.Services
{
    public class ActionService : IActionService
    {
        private readonly ISelectedFileService selectedFileService = null;
        private readonly IAudioPlaybackService audioPlaybackService = null;
        private readonly IAudioRecordingService audioRecordingService = null;
        private readonly IAppSettingService appSettingService = null;

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