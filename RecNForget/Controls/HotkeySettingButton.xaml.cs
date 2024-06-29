using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for HotkeySettingButton.xaml
    /// </summary>
    public partial class HotkeySettingButton : UserControl
    {
        public static readonly DependencyProperty SettingCaptionProperty =
            DependencyProperty.Register("SettingCaption", typeof(string), typeof(HotkeySettingButton), new PropertyMetadata(default(string)));
        public string SettingCaption
        {
            get { return (string)GetValue(SettingCaptionProperty); }
            set { SetValue(SettingCaptionProperty, value); }
        }

        public static readonly DependencyProperty SettingValueProperty =
            DependencyProperty.Register("SettingValue", typeof(string), typeof(HotkeySettingButton), new PropertyMetadata(default(string)));

        public string SettingValue
        {
            get { return (string)GetValue(SettingValueProperty); }
            set { SetValue(SettingValueProperty, value); }
        }

        public HotkeySettingButton()
        {
            InitializeComponent();
        }

        public RelayCommand RelayCommand
        {
            get { return (RelayCommand)GetValue(RelayCommandProperty); }
            set { SetValue(RelayCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RelayCommandProperty =
            DependencyProperty.Register("RelayCommand", typeof(RelayCommand), typeof(HotkeySettingButton), new PropertyMetadata(null));
    }
}
