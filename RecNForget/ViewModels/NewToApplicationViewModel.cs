using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecNForget.Controls;
using RecNForget.Services;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;
using System.Windows;

namespace RecNForget.ViewModels;

public partial class NewToApplicationViewModel : ObservableValidator
{
    private readonly IApplicationHotkeyService hotkeyService;
    private readonly IAppSettingService settingService;
    private readonly IActionService actionService;

    public NewToApplicationViewModel(IApplicationHotkeyService hotkeyService, IAppSettingService settingService, IActionService actionService)
    {
        this.hotkeyService = hotkeyService;
        this.settingService = settingService;
        this.actionService = actionService;

        OutputFolder = settingService.OutputPath;
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
    private void UpdateOutputFolder()
    {
        actionService.ChangeOutputFolder();
        OutputFolder = settingService.OutputPath;
    }

    [RelayCommand]
    private void OpenSettings()
    {
        actionService.ShowSettingsMenu();
    }

    [ObservableProperty]
    private string outputFolder;

    public string HotKey_StartStopRecording
    {
        get => settingService.HotKey_StartStopRecording;
        set
        {
            SetProperty(settingService.HotKey_StartStopRecording, value, settingService, (x, y) => x.HotKey_StartStopRecording = y);
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
}
