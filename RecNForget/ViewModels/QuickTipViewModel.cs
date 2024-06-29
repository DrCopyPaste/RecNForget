using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecNForget.Help;
using RecNForget.Services.Contracts;

namespace RecNForget.ViewModels;

public partial class QuickTipViewModel : ObservableObject
{
    private readonly IAppSettingService settingService;

    public QuickTipViewModel(IAppSettingService settingService)
    {
        this.settingService = settingService;
        GenerateRandomQuickTipCommand.Execute(this);
    }

    [RelayCommand]
    private void SetFeature(HelpFeature helpFeature)
    {
        FeatureCaption = helpFeature.Title;
        FeatureContents = helpFeature.HelpLinesAsString();
    }

    [RelayCommand]
    private void GenerateRandomQuickTip()
    {
        HelpFeature randomFeature = HelpFeature.GetRandomFeature();

        FeatureCaption = randomFeature.Title;
        FeatureContents = randomFeature.HelpLinesAsString();
    }

    [ObservableProperty]
    private string featureCaption;

    [ObservableProperty]
    private string featureContents;

    public bool ShowTipsAtApplicationStart
    {
        get => settingService.ShowTipsAtApplicationStart;
        set
        {
            SetProperty(settingService.ShowTipsAtApplicationStart, value, settingService, (x, y) => x.ShowTipsAtApplicationStart = y);
            settingService.Persist();
        }
    }
}
