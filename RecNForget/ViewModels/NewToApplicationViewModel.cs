using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using RecNForget.Controls;
using RecNForget.Controls.IoC;
using RecNForget.Services.Contracts;
using System.Windows;

namespace RecNForget.ViewModels;

public partial class NewToApplicationViewModel : ObservableValidator
{
    private readonly IApplicationHotkeyService hotkeyService;
    private readonly IAppSettingService settingService;
    private readonly ISelectedFileService selectedFileService;

    public NewToApplicationViewModel(
        IAppSettingService settingService,
        IApplicationHotkeyService hotkeyService,
        ISelectedFileService selectedFileService)
    {
        this.hotkeyService = hotkeyService;
        this.settingService = settingService;
        this.selectedFileService = selectedFileService;

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
        var dialog = new OpenFolderDialog();
        if (!string.IsNullOrEmpty(settingService.OutputPath))
        {
            dialog.DefaultDirectory = settingService.OutputPath;
        }

        var result = dialog.ShowDialog();

        if (result.HasValue && result.Value)
        {
            OutputFolder = dialog.FolderName;
            selectedFileService.SelectLatestFile();
        }
    }

    [RelayCommand]
    private void OpenSettings()
    {
        var appSettingService = ConfiguredServices.ServiceProvider.GetRequiredService<SettingsWindow>();
        appSettingService.ShowDialog();
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
