using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.Wave;
using Notifications.Wpf.Core;
using RecNForget.Services;
using RecNForget.Services.Contracts;
using RecNForget.Services.Contracts.Events;
using RecNForget.WPF.Services.Contracts;
using System;
using System.IO;

namespace RecNForget.ViewModels;

public partial class MainViewModel : ObservableValidator
{
    private readonly NotificationManager notificationManager = new NotificationManager();
    private readonly IActionService actionService;
    private readonly IAppSettingService appSettingService;
    private readonly IAudioRecordingService audioRecordingService;
    private readonly IAudioPlaybackService audioPlaybackService;
    private readonly ISelectedFileService selectedFileService;

    public MainViewModel(
        IActionService actionService,
        IAppSettingService appSettingService,
        IAudioRecordingService audioRecordingService,
        IAudioPlaybackService audioPlaybackService,
        ISelectedFileService selectedFileService)
    {
        this.actionService = actionService;
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
                if (appSettingService.PlayAudioFeedBackMarkingStartAndStopRecording)
                {
                    actionService.QueueAudioPlayback(fileName: audioPlaybackService.RecordStopAudioFeedbackPath);
                }

                if (appSettingService.AutoReplayAudioAfterRecording)
                {
                    actionService.QueueAudioPlayback(
                        fileName: audioRecordingService.LastFileName,
                        startIndicatorFileName: appSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? audioPlaybackService.ReplayStartAudioFeedbackPath : null,
                        endIndicatorFileName: appSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? audioPlaybackService.ReplayStopAudioFeedbackPath : null);
                }

                actionService.TogglePlayPauseAudio();
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
        if (selectedFileService.HasSelectedFile) actionService.ChangeSelectedFileName();
    }

    [RelayCommand]
    private void DeleteSelectedFile()
    {
        if (selectedFileService.HasSelectedFile) actionService.DeleteSelectedFile();
    }

    [RelayCommand]
    private void ExportSelectedFile()
    {
        if (selectedFileService.HasSelectedFile) actionService.ExportSelectedFile();
    }

    [RelayCommand]
    private void SelectInExplorer()
    {
        if (selectedFileService.HasSelectedFile) actionService.OpenOutputFolderInExplorer();
    }

    [RelayCommand]
    private void SelectPreviousFile()
    {
        if (!selectedFileService.SelectPrevFile()) ResetSelectedFile();
    }

    [RelayCommand]
    private void TogglePlaySelectedFile()
    {
        actionService.TogglePlayPauseSelectedFile();
    }

    [RelayCommand]
    private void StopPlaying()
    {
        actionService.StopPlayingSelectedFile();
    }

    [RelayCommand]
    private void SelectNextFile()
    {
        if (!selectedFileService.SelectNextFile()) ResetSelectedFile();
    }

    [RelayCommand]
    private void ToggleRecording()
    {
        actionService.ToggleStartStopRecording();
    }

    [RelayCommand]
    private void UpdateFileNamePattern()
    {
        actionService.ChangeFileNamePattern();
        ProjectedOutputPathIncludingFilePattern = audioRecordingService.GetTargetPathTemplateString();
    }

    [RelayCommand]
    private void UpdateOutputFolder()
    {
        actionService.ChangeOutputFolder();
        ProjectedOutputPathIncludingFilePattern = audioRecordingService.GetTargetPathTemplateString();
    }

    private void ResetSelectedFile()
    {
        HasSelectedFile = false;
        SelectedFilePath = "(no file found or selected)";
    }

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
