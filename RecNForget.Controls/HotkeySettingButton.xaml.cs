using RecNForget.Controls.Helper;
using RecNForget.IoC;
using RecNForget.Services.Contracts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for HotkeySettingButton.xaml
    /// </summary>
    public partial class HotkeySettingButton : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SettingCaptionProperty =
            DependencyProperty.Register("SettingCaption", typeof(string), typeof(HotkeySettingButton), new PropertyMetadata(default(string)));
        public string SettingCaption
        {
            get { return (string)GetValue(SettingCaptionProperty); }
            set { SetValue(SettingCaptionProperty, value); OnPropertyChanged(); }
        }

        public static readonly DependencyProperty SettingValueProperty =
            DependencyProperty.Register("SettingValue", typeof(string), typeof(HotkeySettingButton), new PropertyMetadata(default(string)));

        private readonly IApplicationHotkeyService hotkeyService;

        public string SettingValue
        {
            get { return (string)GetValue(SettingValueProperty); }
            set
            {
                SetValue(SettingValueProperty, value);
                OnPropertyChanged();
            }
        }

        public HotkeySettingButton()
        {
            InitializeComponent();
            this.hotkeyService = UnityHandler.UnityContainer.Resolve<IApplicationHotkeyService>();

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
            var dialog = new HotkeyPromptWindow("Configure start/stop recording hotkey");

            var parentWindow = Window.GetWindow(this);
            dialog.Owner = parentWindow;

            if (dialog.ShowDialog() == true)
            {
                SettingValue = dialog.HotkeysAppSetting;
                this.hotkeyService.ResetAndReadHotkeysFromConfig();
            }
        }
    }
}
