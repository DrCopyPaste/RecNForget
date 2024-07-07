using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using NAudio.Wave;
using Notifications.Wpf.Core;
using RecNForget.Controls;
using RecNForget.Services;
using RecNForget.Services.Contracts;
using RecNForget.Services.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RecNForget.ViewModels;

public partial class MainViewModel : ObservableValidator
{
    private readonly NotificationManager notificationManager = new NotificationManager();
    private readonly IAppSettingService appSettingService;
    private readonly IAudioRecordingService audioRecordingService;
    private readonly IAudioPlaybackService audioPlaybackService;
    private readonly ISelectedFileService selectedFileService;

    public MainViewModel(
        IAppSettingService appSettingService,
        IAudioRecordingService audioRecordingService,
        IAudioPlaybackService audioPlaybackService,
        ISelectedFileService selectedFileService)
    {
        this.appSettingService = appSettingService;
        this.audioRecordingService = audioRecordingService;
        this.audioPlaybackService = audioPlaybackService;
        this.selectedFileService = selectedFileService;

        selectedFileService.SelectedFileChanged += SelectedFileService_SelectedFileChanged;
        audioPlaybackService.AudioPlaybackChanged += AudioPlaybackService_AudioPlaybackChanged;
        audioRecordingService.AudioRecordingStateChanged += AudioRecordingService_AudioRecordingStateChanged;

        TaskBar_ProgressState = "None";
        ProjectedOutputPathIncludingFilePattern = audioRecordingService.GetTargetPathTemplateString();
        RecordButtonEnabled = true;

        selectedFileService.SelectLatestFile();

        OutputPathControlVisible = appSettingService.OutputPathControlVisible;
        SelectedFileControlVisible = appSettingService.SelectedFileControlVisible;
        RecordingTimerControlVisible = appSettingService.RecordingTimerControlVisible;

    }

    ~MainViewModel()
    {
        selectedFileService.SelectedFileChanged -= SelectedFileService_SelectedFileChanged;
        audioPlaybackService.AudioPlaybackChanged -= AudioPlaybackService_AudioPlaybackChanged;
        audioRecordingService.AudioRecordingStateChanged -= AudioRecordingService_AudioRecordingStateChanged;
    }

    private void AudioRecordingService_AudioRecordingStateChanged(object sender, AudioRecordingServiceEventArgs e)
    {
        PlayPauseButtonEnabled = !e.IsRecording && selectedFileService.HasSelectedFile;
        StopButtonEnabled = false;
        SkipPrevButtonEnabled = !e.IsRecording && selectedFileService.HasSelectedFile;
        SkipNextButtonEnabled = !e.IsRecording && selectedFileService.HasSelectedFile;

        CurrentlyRecording = e.IsRecording;
        CurrentlyNotRecording = !e.IsRecording;

        if (e.IsRecording)
        {
            if (appSettingService.PlayAudioFeedBackMarkingStartAndStopRecording)
            {
                audioPlaybackService.KillAudio(reset: true);

                audioPlaybackService.QueueFile(audioPlaybackService.RecordStartAudioFeedbackPath);
                audioPlaybackService.Play();

                while (audioPlaybackService.PlaybackState != PlaybackState.Stopped) { }

                audioPlaybackService.KillAudio(reset: true);
            }

            if (appSettingService.ShowBalloonTipsForRecording)
            {
                notificationManager.ShowAsync(
                    new NotificationContent()
                    {
                        Type = NotificationType.Information,
                        Title = "Recording started!",
                        Message = "RecNForget now recording..."
                    });
            }

            audioPlaybackService.KillAudio(reset: true);
            TaskBar_ProgressState = "Error";
        }
        else
        {
            if (appSettingService.PlayAudioFeedBackMarkingStartAndStopRecording || appSettingService.AutoReplayAudioAfterRecording)
            {
                var fileQueue = new List<string>();

                if (appSettingService.PlayAudioFeedBackMarkingStartAndStopRecording)
                {
                    fileQueue.Add(audioPlaybackService.RecordStopAudioFeedbackPath);
                }

                

                if (appSettingService.AutoReplayAudioAfterRecording)
                {
                    if (appSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying)
                    {
                        fileQueue.Add(audioPlaybackService.ReplayStartAudioFeedbackPath);
                    }

                    fileQueue.Add(audioRecordingService.LastFileName);

                    if (appSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying)
                    {
                        fileQueue.Add(audioPlaybackService.ReplayStopAudioFeedbackPath);
                    }
                }

                audioPlaybackService.QueueFiles(fileQueue.ToArray());
                TogglePlaySelectedFileCommand.Execute(this);
            }

            TaskBar_ProgressState = "None";

            if (appSettingService.ShowBalloonTipsForRecording)
            {
                notificationManager.ShowAsync(
                    content: new NotificationContent()
                    {
                        Type = NotificationType.Success,
                        Title = "Recording saved!",
                        Message = audioRecordingService.LastFileName
                    },
                    onClick: () =>
                    {
                        if (audioRecordingService.LastFileName == string.Empty || !File.Exists(audioRecordingService.LastFileName))
                        {
                            return;
                        }

                        string argument = "/select, \"" + audioRecordingService.LastFileName + "\"";
                        System.Diagnostics.Process.Start("explorer.exe", argument);
                    });
            }

            if (appSettingService.AutoSelectLastRecording)
            {
                selectedFileService.SelectFile(new FileInfo(audioRecordingService.LastFileName));
            }
        }
    }

    private void AudioPlaybackService_AudioPlaybackChanged(object sender, AudioPlaybackServiceEventArgs e)
    {
        Playing = e.PlaybackState == PlaybackState.Playing;
        Paused = e.PlaybackState == PlaybackState.Paused;
        PlayingOrPaused = e.PlaybackState == PlaybackState.Playing || e.PlaybackState == PlaybackState.Paused;
        Stopped = e.PlaybackState == PlaybackState.Stopped;

        RecordButtonEnabled = e.PlaybackState == PlaybackState.Stopped;
    }

    private void SelectedFileService_SelectedFileChanged(object sender, SelectedFileServiceEventArgs e)
    {
        HasSelectedFile = e.HasFileSelected;
        SelectedFilePath = e.HasFileSelected ? e.FileName : "(no file found or selected)";

        PlayPauseButtonEnabled = e.HasFileSelected;
        StopButtonEnabled = e.HasFileSelected;
        SkipPrevButtonEnabled = e.HasFileSelected;
        SkipNextButtonEnabled = e.HasFileSelected;

        try
        {
            var audioFileLengthString = audioPlaybackService.GetFileLengthInSecondsFormatted(selectedFileService.SelectedFile.FullName);
            var fileSizeString = (selectedFileService.SelectedFile.Length / (double)1024).ToString("N2") + " kB";

            FileInfoText = audioFileLengthString + " (" + fileSizeString + ")";
        }
        catch (Exception ex)
        {
            FileInfoText = "error trying to read file size";

            notificationManager.ShowAsync(
                content: new NotificationContent()
                {
                    Title = "Error trying to read file size",
                    Message = "an error occurred while trying to parse audio file: " + ex.Message,
                    Type = NotificationType.Error
                },
                expirationTime: TimeSpan.FromSeconds(10));
        }
    }

    [RelayCommand]
    private void ChangeSelectedFileName()
    {
        if (!selectedFileService.HasSelectedFile) return;

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

        // tempDialog.TrySetViewablePositionFromOwner(OwnerControl);

        if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
        {
            if (!selectedFileService.RenameSelectedFileWithoutExtension(tempDialog.PromptContent))
            {
                notificationManager.ShowAsync(
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

    [RelayCommand]
    private void DeleteSelectedFile()
    {
        if (!selectedFileService.HasSelectedFile) return;

        audioPlaybackService.Stop();
        audioPlaybackService.KillAudio();

        CustomMessageBox tempDialog = new CustomMessageBox(
            caption: "Are you sure you want to delete this file?",
            icon: CustomMessageBoxIcon.Question,
            buttons: CustomMessageBoxButtons.OkAndCancel,
            messageRows: new List<string>() { selectedFileService.SelectedFile.FullName },
            controlFocus: CustomMessageBoxFocus.Ok);

        //tempDialog.TrySetViewablePositionFromOwner(OwnerControl);

        if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
        {
            if (!selectedFileService.DeleteSelectedFile())
            {
                notificationManager.ShowAsync(
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

    [RelayCommand]
    private void ExportSelectedFile()
    {
        if (!selectedFileService.HasSelectedFile) return;

        var preferredFileName = string.Empty;

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

            // tempDialog.TrySetViewablePositionFromOwner(OwnerControl);

            if (!tempDialog.ShowDialog().HasValue || !tempDialog.Ok)
            {
                return;
            }

            preferredFileName = tempDialog.PromptContent;
        }

        var task = Task.Run(() =>
        {
            notificationManager.ShowAsync(
              content: new NotificationContent()
              {
                  Type = NotificationType.Information,
                  Title = $"Exporting {selectedFileService.SelectedFile.Name} MP3 @ {appSettingService.Mp3ExportBitrate} kbps",
                  Message = $"Export has started, this may take a moment..."
              });

            var exportedFileName = selectedFileService.ExportFile(preferredFileName);

            if (string.IsNullOrEmpty(exportedFileName))
            {
                notificationManager.ShowAsync(
                    content: new NotificationContent()
                    {
                        Title = "Something went wrong",
                        Message = "An unknown error occurred trying to export the selected file",
                        Type = NotificationType.Error
                    },
                    expirationTime: TimeSpan.FromSeconds(10));
                return;
            }

            notificationManager.ShowAsync(
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
        });
    }

    [RelayCommand]
    private void SelectInExplorer()
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

    [RelayCommand]
    private void SelectPreviousFile()
    {
        if (!selectedFileService.SelectPrevFile()) ResetSelectedFile();
    }

    [RelayCommand]
    private void TogglePlaySelectedFile()
    {
        if (audioPlaybackService.ItemsCount == 0)
        {
            if (appSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying)
            {
                audioPlaybackService.QueueFile(audioPlaybackService.ReplayStartAudioFeedbackPath);
            }

            audioPlaybackService.QueueFile(selectedFileService.SelectedFile.FullName);

            if (appSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying)
            {
                audioPlaybackService.QueueFile(audioPlaybackService.ReplayStopAudioFeedbackPath);
            }
        }

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

    [RelayCommand]
    private void StopPlaying()
    {
        audioPlaybackService.Stop();
    }

    [RelayCommand]
    private void SelectNextFile()
    {
        if (!selectedFileService.SelectNextFile()) ResetSelectedFile();
    }

    [RelayCommand]
    private void ToggleRecording()
    {
        audioRecordingService.ToggleRecording();
    }

    [RelayCommand]
    private void UpdateFileNamePattern()
    {
        CustomMessageBox tempDialog = new CustomMessageBox(
                caption: "Type in a new pattern for file name generation.",
                icon: CustomMessageBoxIcon.Question,
                buttons: CustomMessageBoxButtons.OkAndCancel,
                messageRows: new List<string>() { "Supported placeholders:", "(Date), (Guid)", "If you do not provide a placeholder to create unique file names, RecNForget will do it for you." },
                prompt: appSettingService.FilenamePrefix,
                controlFocus: CustomMessageBoxFocus.Prompt,
                promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

        // tempDialog.TrySetViewablePositionFromOwner(OwnerControl);

        if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
        {
            appSettingService.FilenamePrefix = tempDialog.PromptContent;
            appSettingService.Persist();
        }

        ProjectedOutputPathIncludingFilePattern = audioRecordingService.GetTargetPathTemplateString();
    }

    [RelayCommand]
    private void UpdateOutputFolder()
    {
        var dialog = new OpenFolderDialog();
        if (!string.IsNullOrEmpty(appSettingService.OutputPath))
        {
            dialog.DefaultDirectory = appSettingService.OutputPath;
        }

        var result = dialog.ShowDialog();

        if (result.HasValue && result.Value)
        {
            appSettingService.OutputPath = dialog.FolderName;
            appSettingService.Persist();

            selectedFileService.SelectLatestFile();
        }

        ProjectedOutputPathIncludingFilePattern = audioRecordingService.GetTargetPathTemplateString();
    }

    private void ResetSelectedFile()
    {
        HasSelectedFile = false;
        SelectedFilePath = "(no file found or selected)";
    }

    [ObservableProperty]
    private bool outputPathControlVisible;

    [ObservableProperty]
    private bool selectedFileControlVisible;

    [ObservableProperty]
    private bool recordingTimerControlVisible;

    [ObservableProperty]
    private bool skipPrevButtonEnabled;

    [ObservableProperty]
    private bool playPauseButtonEnabled;

    [ObservableProperty]
    private bool stopButtonEnabled;

    [ObservableProperty]
    private bool skipNextButtonEnabled;

    [ObservableProperty]
    private bool recordButtonEnabled;

    [ObservableProperty]
    private bool playing;

    [ObservableProperty]
    private bool paused;

    [ObservableProperty]
    private bool playingOrPaused;

    [ObservableProperty]
    private bool stopped;

    [ObservableProperty]
    private bool currentlyRecording;

    [ObservableProperty]
    private bool currentlyNotRecording;

    [ObservableProperty]
    private string taskBar_ProgressState;

    [ObservableProperty]
    private string projectedOutputPathIncludingFilePattern;

    [ObservableProperty]
    private bool hasSelectedFile;

    [ObservableProperty]
    private string fileInfoText;

    [ObservableProperty]
    private string selectedFilePath;
}
