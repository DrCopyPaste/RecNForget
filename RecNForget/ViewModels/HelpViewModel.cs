using CommunityToolkit.Mvvm.ComponentModel;
using RecNForget.Help;
using RecNForget.Services.Contracts;
using System.Collections.ObjectModel;
using System.Linq;

namespace RecNForget.ViewModels;

public partial class HelpViewModel : ObservableObject
{
    private readonly IAppSettingService appSettingService;

    public HelpViewModel(IAppSettingService appSettingService)
    {
        this.appSettingService = appSettingService;

        Features.Add(new Help.General.QuickStart());
        foreach (var feature in HelpFeature.All.Where(f => f.FeatureClass == HelpFeatureClass.NewFeature))
        {
            Features.Add(feature);
        }
    }

    [ObservableProperty]
    private ObservableCollection<HelpFeature> features = new ObservableCollection<HelpFeature>();
}
