using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Notifications.Wpf.Core;
using RecNForget.Controls;
using RecNForget.Services;
using RecNForget.Services.Contracts;
using RecNForget.Services.Contracts.Events;
using RecNForget.WPF.Services.Contracts;
using System;
using System.IO;
using System.Windows;

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
        TaskBar_ProgressState = "None";
        ProjectedOutputPathIncludingFilePattern = audioRecordingService.GetTargetPathTemplateString();

        selectedFileService.SelectLatestFile();
    }

    ~MainViewModel()
    {
        selectedFileService.SelectedFileChanged -= SelectedFileService_SelectedFileChanged;
    }

    private void SelectedFileService_SelectedFileChanged(object sender, SelectedFileServiceEventArgs e)
    {
        HasSelectedFile = e.HasFileSelected;
        SelectedFilePath = e.HasFileSelected ? e.FileName : "(no file found or selected)";

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
        actionService.ChangeSelectedFileName();
    }

    [RelayCommand]
    private void DeleteSelectedFile()
    {
        actionService.DeleteSelectedFile();
    }

    [RelayCommand]
    private void ExportSelectedFile()
    {
        actionService.ExportSelectedFile();
    }

    [RelayCommand]
    private void SelectPreviousFile()
    {
        if (!selectedFileService.SelectPrevFile()) ResetSelectedFile();
    }

    [RelayCommand]
    private void SelectNextFile()
    {
        if (!selectedFileService.SelectNextFile()) ResetSelectedFile();
    }

    //[RelayCommand]
    //private void SelectFile(FileInfo file)
    //{
    //    if (selectedFileService.SelectFile(file))
    //    {
    //        ResetSelectedFile();
    //        return;
    //    }

    //    HasSelectedFile = true;
    //    SelectedFileDisplay = selectedFileService.SelectedFile.Name;

    //}

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
