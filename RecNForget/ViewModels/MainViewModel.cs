using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;

namespace RecNForget.ViewModels;

public partial class MainViewModel : ObservableValidator
{
    private readonly IActionService actionService;
    private readonly IAppSettingService appSettingService;
    private readonly IAudioRecordingService audioRecordingService;

    public MainViewModel(IActionService actionService, IAppSettingService appSettingService, IAudioRecordingService audioRecordingService)
    {
        this.actionService = actionService;
        this.appSettingService = appSettingService;
        this.audioRecordingService = audioRecordingService;

        ProjectedOutputPathIncludingFilePattern = audioRecordingService.GetTargetPathTemplateString();
    }

    [ObservableProperty]
    private string projectedOutputPathIncludingFilePattern;

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
}
