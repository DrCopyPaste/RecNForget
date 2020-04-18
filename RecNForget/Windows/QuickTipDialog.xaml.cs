using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.Services.Types;

namespace RecNForget.Windows
{
    /// <summary>
    /// Interaction logic for QuickTipDialog.xaml
    /// </summary>
    public partial class QuickTipDialog : INotifyPropertyChanged
    {
        private IAppSettingService settingService;
        private string featureCaption;
        private string featureContents;

        public QuickTipDialog(IAppSettingService settingService)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                SettingService = new DesignerAppSettingService();
            }
            else
            {
                SettingService = settingService;
            }

            this.KeyDown += Window_KeyDown;

            InitializeComponent();
            DataContext = this;

            this.Title = "Did you know?";

            GenerateRandomTip();
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

        public string FeatureCaption
        {
            get
            {
                return featureCaption;
            }

            set
            {
                featureCaption = value;
                OnPropertyChanged();
            }
        }

        public string FeatureContents
        {
            get
            {
                return featureContents;
            }

            set
            {
                featureContents = value;
                OnPropertyChanged();
            }
        }

        private void GenerateRandomTip()
        {
            var allFeatures = Services.Types.HelpFeature.All.Where(f => f.FeatureClass == HelpFeatureClass.NewFeature || f.FeatureClass == HelpFeatureClass.FunFact).ToList();

            int randomNumber = (new Random()).Next(0, allFeatures.Count - 1);
            var randomFeature = allFeatures[randomNumber];

            FeatureCaption = randomFeature.Title;
            FeatureContents = randomFeature.HelpLinesAsString();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void GenerateAnotherTip_Click(object sender, RoutedEventArgs e)
        {
            GenerateRandomTip();
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
