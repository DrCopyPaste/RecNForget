using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using RecNForget.Help;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for QuickTipDialog.xaml
    /// </summary>
    public partial class QuickTipDialog : INotifyPropertyChanged
    {
        private IAppSettingService settingService;
        private string featureCaption;
        private string featureContents;

        public QuickTipDialog(IAppSettingService settingService, HelpFeature randomFeature)
        {
            DataContext = this;
            InitializeComponent();

            this.Title = "Did you know?";

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
            }

            SetContents(randomFeature);
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
            HelpFeature randomFeature = HelpFeature.GetRandomFeature();
            SetContents(randomFeature);
        }

        private void SetContents(HelpFeature randomFeature)
        {
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
