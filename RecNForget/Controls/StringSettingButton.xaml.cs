using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for StringSettingButton.xaml
    /// </summary>
    public partial class StringSettingButton : UserControl, INotifyPropertyChanged
    {
        public event RoutedEventHandler Click;

        public static readonly DependencyProperty SettingCaptionProperty =
            DependencyProperty.Register("SettingCaption", typeof(string), typeof(StringSettingButton), new PropertyMetadata(string.Empty));
        public string SettingCaption
        {
            get { return (string)GetValue(SettingCaptionProperty); }
            set { SetValue(SettingCaptionProperty, value); OnPropertyChanged(); }
        }

        public static readonly DependencyProperty SettingValueProperty =
            DependencyProperty.Register("SettingValue", typeof(string), typeof(StringSettingButton), new PropertyMetadata(string.Empty));
        public string SettingValue
        {
            get { return (string)GetValue(SettingValueProperty); }
            set { SetValue(SettingValueProperty, value); OnPropertyChanged(); }
        }

        public StringSettingButton()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            { }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Click_Internal(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(sender, e);
        }
    }
}
