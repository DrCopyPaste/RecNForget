using CommunityToolkit.Mvvm.ComponentModel;
using RecNForget.Help;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RecNForget.ViewModels;

public partial class NewToVersionViewModel : ObservableObject
{
    private IEnumerable<HelpFeature> verboseInformationFeatures;
    private IEnumerable<HelpFeature> addedFeatures;
    private IEnumerable<HelpFeature> bugFixes;

    [ObservableProperty]
    private Version lastInstalledVersion;

    [ObservableProperty]
    private Version currentInstalledVersion;

    [ObservableProperty]
    private ObservableCollection<HelpFeature> patchNotes;

    public NewToVersionViewModel()
    {
        var featuresSinceLastVersion = HelpFeature.All.Where(f => f.FeatureClass != HelpFeatureClass.FunFact && f.MinVersion != null && f.MinVersion.CompareTo(LastInstalledVersion) > 0).OrderBy(f => f.Priority);

        // ToDo show disabled features in this dialog ?!
        var disabledFeaturesSinceLastVersion = featuresSinceLastVersion.Where(f => f.MaxVersion != null && f.MaxVersion.CompareTo(CurrentInstalledVersion) < 0);
        var addedFeaturesSinceLastVersion = featuresSinceLastVersion.Except(disabledFeaturesSinceLastVersion).OrderBy(x => x.Priority).ThenByDescending(x => x.FeatureClass);

        PatchNotes = new ObservableCollection<HelpFeature>(addedFeaturesSinceLastVersion);
    }
}
