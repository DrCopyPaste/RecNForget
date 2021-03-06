﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecNForget.Controls.Extensions;
using RecNForget.Help;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for NewToVersionDialog.xaml
    /// </summary>
    public partial class NewToVersionDialog : INotifyPropertyChanged
    {
        private IEnumerable<HelpFeature> verboseInformationFeatures;
        private IEnumerable<HelpFeature> addedFeatures;
        private IEnumerable<HelpFeature> bugFixes;

        private IAppSettingService settingService;

        public NewToVersionDialog(Version lastInstalledVersion, Version currentVersion, IAppSettingService settingService)
        {
            InitializeComponent();
            DataContext = this;

            this.Title = "Things changed since we last met";

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                SettingService = new DesignerAppSettingService();
            }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;

                SettingService = settingService;

                this.KeyDown += Window_KeyDown;

                var featuresSinceLastVersion = HelpFeature.All.Where(f => f.MinVersion != null && f.MinVersion.CompareTo(lastInstalledVersion) > 0).OrderBy(f => f.Priority);
                var disabledFeaturesSinceLastVersion = featuresSinceLastVersion.Where(f => f.MaxVersion != null && f.MaxVersion.CompareTo(currentVersion) < 0);
                var addedFeaturesSinceLastVersion = featuresSinceLastVersion.Except(disabledFeaturesSinceLastVersion);

                // we dont show disabled features in this dialog
                // major feature changes are communicated by Information feature class
                this.verboseInformationFeatures = addedFeaturesSinceLastVersion.Where(f => f.FeatureClass == HelpFeatureClass.Information);
                this.addedFeatures = addedFeaturesSinceLastVersion.Where(f => f.FeatureClass == HelpFeatureClass.NewFeature);
                this.bugFixes = addedFeaturesSinceLastVersion.Where(f => f.FeatureClass == HelpFeatureClass.BugFix);

                foreach (var feature in verboseInformationFeatures)
                {
                    AddFeatureToDisplayList(feature, VersionInfoFeatureList);
                }

                foreach (var feature in bugFixes)
                {
                    AddFeatureToDisplayList(feature, VersionInfoFeatureList);
                }

                foreach (var feature in addedFeatures)
                {
                    AddFeatureToDisplayList(feature, VersionInfoFeatureList);
                }
            }

           
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IAppSettingService SettingService
        {
            get
            {
                return settingService;
            }

            set
            {
                settingService = value;
                OnPropertyChanged();
            }
        }

        private void AddFeatureToDisplayList(HelpFeature feature, Grid targetGrid)
        {
            var featureRowDefinition = new RowDefinition();
            featureRowDefinition.Height = GridLength.Auto;
            targetGrid.RowDefinitions.Add(featureRowDefinition);

            Style textBlockStyle = (Style)FindResource("DefaultTextBlockStyle");
            Style addedFeatureImageStyle = (Style)FindResource("AddedFeature_Image_Style");
            Style bugfixImageStyle = (Style)FindResource("BugFix_Image_Style");
            Style verboseInfoImageStyle = (Style)FindResource("VerboseInformationFeature_Image_Style");

            var featureText = new TextBlock();
            featureText.Style = textBlockStyle;
            featureText.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            featureText.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            featureText.Text = feature.Title;

            var detailsHelpText = feature.HelpLinesAsString();

            switch (feature.FeatureClass)
            {
                case HelpFeatureClass.Information:
                {
                    if (verboseInfoImageStyle != null)
                    {
                        var featureType = new SVGImage.SVG.SVGImage();
                        featureType.Style = verboseInfoImageStyle;
                        featureType.SetCustomToolTip("important information");

                        targetGrid.InsertAt(featureType, 0, targetGrid.RowDefinitions.Count - 1);
                    }

                    break;
                }

                case HelpFeatureClass.NewFeature:
                {
                    if (addedFeatureImageStyle != null)
                    {
                        var featureType = new SVGImage.SVG.SVGImage();
                        featureType.Style = addedFeatureImageStyle;
                        featureType.SetCustomToolTip("added feature");

                        targetGrid.InsertAt(featureType, 0, targetGrid.RowDefinitions.Count - 1);
                    }

                    break;
                }

                case HelpFeatureClass.BugFix:
                {
                    if (bugfixImageStyle != null)
                    {
                        var featureType = new SVGImage.SVG.SVGImage();
                        featureType.Style = bugfixImageStyle;
                        featureType.SetCustomToolTip("bug fix");

                        targetGrid.InsertAt(featureType, 0, targetGrid.RowDefinitions.Count - 1);
                    }

                    break;
                }
            }

            featureText.SetCustomToolTip(detailsHelpText);

            targetGrid.InsertAt(featureText, 1, targetGrid.RowDefinitions.Count - 1);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
