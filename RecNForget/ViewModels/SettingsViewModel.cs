using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using RecNForget.Controls;
using RecNForget.Controls.Services;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RecNForget.ViewModels;

public partial class SettingsViewModel : ObservableValidator
{
    private readonly IActionService actionService;
    private readonly IAppSettingService settingService;
    private readonly IApplicationHotkeyService hotkeyService;

    public SettingsViewModel(IActionService actionService, IAppSettingService settingService, IApplicationHotkeyService hotkeyService)
    {
        this.actionService = actionService;
        this.settingService = settingService;
        this.hotkeyService = hotkeyService;
    }

    [RelayCommand]
    private void UpdateToggleRecordingHotkey()
    {
        var dialog = new HotkeyPromptWindow("Configure start/stop recording hotkey");

        var parentWindow = Window.GetWindow(new DependencyObject());
        dialog.Owner = parentWindow;

        if (dialog.ShowDialog() == true)
        {
            HotKey_StartStopRecording = dialog.HotkeysAppSetting;
            hotkeyService.ResetAndReadHotkeysFromConfig();
        }
    }

    [RelayCommand]
    private void Configure_OutputPath()
    {
        actionService.ChangeOutputFolder();
    }

    [RelayCommand]
    private void Configure_ExportOutputPath()
    {
        var dialog = new OpenFolderDialog();
        if (!string.IsNullOrEmpty(ExportOutputPath))
        {
            dialog.DefaultDirectory = ExportOutputPath;
        };

        if (dialog.ShowDialog() == true)
        {
            ExportOutputPath = dialog.FolderName;
        }
    }

    [RelayCommand]
    private void Configure_FileNamePattern()
    {
        CustomMessageBox tempDialog = new CustomMessageBox(
            caption: "Type in a new pattern for file name generation.",
            icon: CustomMessageBoxIcon.Question,
            buttons: CustomMessageBoxButtons.OkAndCancel,
            messageRows: new List<string>() { "Supported placeholders:", "(Date)" },
            prompt: FilenamePrefix,
            controlFocus: CustomMessageBoxFocus.Prompt,
            promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

        if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
        {
            FilenamePrefix = tempDialog.PromptContent;
        }
    }

    public string RuntimeVersionString { get; }
    public string RuntimeInformalVersionString { get; }


    public bool AutoStartWithWindows
    {
        get => settingService.AutoStartWithWindows;
        set
        {
            SetProperty(settingService.AutoStartWithWindows, value, settingService, (x,y) => x.AutoStartWithWindows = y);
            settingService.Persist();
        }
    }

    public bool CheckForUpdateOnStart
    {
        get => settingService.CheckForUpdateOnStart;
        set
        {
            SetProperty(settingService.CheckForUpdateOnStart, value, settingService, (x, y) => x.CheckForUpdateOnStart = y);
            settingService.Persist();
        }
    }

    public bool AutoSelectLastRecording
    {
        get => settingService.AutoSelectLastRecording;
        set
        {
            SetProperty(settingService.AutoSelectLastRecording, value, settingService, (x, y) => x.AutoSelectLastRecording = y);
            settingService.Persist();
        }
    }

    public bool AutoReplayAudioAfterRecording
    {
        get => settingService.AutoReplayAudioAfterRecording;
        set
        {
            SetProperty(settingService.AutoReplayAudioAfterRecording, value, settingService, (x, y) => x.AutoReplayAudioAfterRecording = y);
            settingService.Persist();
        }
    }

    public bool PlayAudioFeedBackMarkingStartAndStopReplaying
    {
        get => settingService.PlayAudioFeedBackMarkingStartAndStopReplaying;
        set
        {
            SetProperty(settingService.PlayAudioFeedBackMarkingStartAndStopReplaying, value, settingService, (x, y) => x.PlayAudioFeedBackMarkingStartAndStopReplaying = y);
            settingService.Persist();
        }
    }

    public bool PlayAudioFeedBackMarkingStartAndStopRecording
    {
        get => settingService.PlayAudioFeedBackMarkingStartAndStopRecording;
        set
        {
            SetProperty(settingService.PlayAudioFeedBackMarkingStartAndStopRecording, value, settingService, (x, y) => x.PlayAudioFeedBackMarkingStartAndStopRecording = y);
            settingService.Persist();
        }
    }

    public bool MinimizedToTray
    {
        get => settingService.MinimizedToTray;
        set
        {
            SetProperty(settingService.MinimizedToTray, value, settingService, (x, y) => x.MinimizedToTray = y);
            settingService.Persist();
        }
    }

    public string HotKey_StartStopRecording
    {
        get => settingService.HotKey_StartStopRecording;
        set
        {
            SetProperty(settingService.HotKey_StartStopRecording, value, settingService, (x, y) => x.HotKey_StartStopRecording = y);
            settingService.Persist();
        }
    }

    public string FilenamePrefix
    {
        get => settingService.FilenamePrefix;
        set
        {
            SetProperty(settingService.FilenamePrefix, value, settingService, (x, y) => x.FilenamePrefix = y);
            settingService.Persist();
        }
    }

    public string OutputPath
    {
        get => settingService.OutputPath;
        set
        {
            SetProperty(settingService.OutputPath, value, settingService, (x, y) => x.OutputPath = y);
            settingService.Persist();
        }
    }

    public bool WindowAlwaysOnTop
    {
        get => settingService.WindowAlwaysOnTop;
        set
        {
            SetProperty(settingService.WindowAlwaysOnTop, value, settingService, (x, y) => x.WindowAlwaysOnTop = y);
            settingService.Persist();
        }
    }

    public bool ShowBalloonTipsForRecording
    {
        get => settingService.ShowBalloonTipsForRecording;
        set
        {
            SetProperty(settingService.ShowBalloonTipsForRecording, value, settingService, (x, y) => x.ShowBalloonTipsForRecording = y);
            settingService.Persist();
        }
    }

    public bool ShowTipsAtApplicationStart
    {
        get => settingService.ShowTipsAtApplicationStart;
        set
        {
            SetProperty(settingService.ShowTipsAtApplicationStart, value, settingService, (x, y) => x.ShowTipsAtApplicationStart = y);
            settingService.Persist();
        }
    }

    public Version LastInstalledVersion
    {
        get => settingService.LastInstalledVersion;
        set
        {
            SetProperty(settingService.LastInstalledVersion, value, settingService, (x, y) => x.LastInstalledVersion = y);
            settingService.Persist();
        }
    }

    public double? MainWindowLeftX
    {
        get => settingService.MainWindowLeftX;
        set
        {
            SetProperty(settingService.MainWindowLeftX, value, settingService, (x, y) => x.MainWindowLeftX = y);
            settingService.Persist();
        }
    }

    public double? MainWindowTopY
    {
        get => settingService.MainWindowTopY;
        set
        {
            SetProperty(settingService.MainWindowTopY, value, settingService, (x, y) => x.MainWindowTopY = y);
            settingService.Persist();
        }
    }

    public bool OutputPathControlVisible
    {
        get => settingService.OutputPathControlVisible;
        set
        {
            SetProperty(settingService.OutputPathControlVisible, value, settingService, (x, y) => x.OutputPathControlVisible = y);
            settingService.Persist();
        }
    }

    public bool SelectedFileControlVisible
    {
        get => settingService.SelectedFileControlVisible;
        set
        {
            SetProperty(settingService.SelectedFileControlVisible, value, settingService, (x, y) => x.SelectedFileControlVisible = y);
            settingService.Persist();
        }
    }

    public string WindowTheme
    {
        get => settingService.WindowTheme;
        set
        {
            SetProperty(settingService.WindowTheme, value, settingService, (x, y) => x.WindowTheme = y);
            settingService.Persist();
        }
    }

    public double UiScalingPercent
    {
        get => settingService.UiScalingPercent;
        set
        {
            SetProperty(settingService.UiScalingPercent, value, settingService, (x, y) => x.UiScalingPercent = y);
            settingService.Persist();
        }
    }

    public int Mp3ExportBitrate
    {
        get => settingService.Mp3ExportBitrate;
        set
        {
            SetProperty(settingService.Mp3ExportBitrate, value, settingService, (x, y) => x.Mp3ExportBitrate = y);
            settingService.Persist();
        }
    }

    public bool PromptForExportFileName
    {
        get => settingService.PromptForExportFileName;
        set
        {
            SetProperty(settingService.PromptForExportFileName, value, settingService, (x, y) => x.PromptForExportFileName = y);
            settingService.Persist();
        }
    }

    public bool RecordingTimerStopAfterIsEnabled
    {
        get => settingService.RecordingTimerStopAfterIsEnabled;
        set
        {
            SetProperty(settingService.RecordingTimerStopAfterIsEnabled, value, settingService, (x, y) => x.RecordingTimerStopAfterIsEnabled = y);
            settingService.Persist();
        }
    }

    public bool RecordingTimerStartAfterIsEnabled
    {
        get => settingService.RecordingTimerStartAfterIsEnabled;
        set
        {
            SetProperty(settingService.RecordingTimerStartAfterIsEnabled, value, settingService, (x, y) => x.RecordingTimerStartAfterIsEnabled = y);
            settingService.Persist();
        }
    }

    public bool RecordingTimerControlVisible
    {
        get => settingService.RecordingTimerControlVisible;
        set
        {
            SetProperty(settingService.RecordingTimerControlVisible, value, settingService, (x, y) => x.RecordingTimerControlVisible = y);
            settingService.Persist();
        }
    }

    public string RecordingTimerStartAfterMax
    {
        get => settingService.RecordingTimerStartAfterMax;
        set
        {
            SetProperty(settingService.RecordingTimerStartAfterMax, value, settingService, (x, y) => x.RecordingTimerStartAfterMax = y);
            settingService.Persist();
        }
    }

    public string RecordingTimerStopAfterMax
    {
        get => settingService.RecordingTimerStopAfterMax;
        set
        {
            SetProperty(settingService.RecordingTimerStopAfterMax, value, settingService, (x, y) => x.RecordingTimerStopAfterMax = y);
            settingService.Persist();
        }
    }

    public string ExportOutputPath
    {
        get => settingService.ExportOutputPath;
        set
        {
            SetProperty(settingService.ExportOutputPath, value, settingService, (x, y) => x.ExportOutputPath = y);
            settingService.Persist();
        }
    }

}
