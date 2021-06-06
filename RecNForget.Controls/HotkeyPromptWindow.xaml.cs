using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using PressingIssue.Services.Contracts;
using PressingIssue.Services.Contracts.Events;
using RecNForget.Controls.Helper;
using RecNForget.IoC;
using RecNForget.Services.Designer;
using Unity;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for HotkeyPromptWindow.xaml
    /// </summary>
    public partial class HotkeyPromptWindow : Window
    {
        private readonly ISimpleGlobalHotkeyService simpleGlobalHotkeyService = null;

        public HotkeyPromptWindow(string title)
        {
            DataContext = this;
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.simpleGlobalHotkeyService = new DesignerSimpleGlobalHotkeyService();
            }
            else
            {
                this.Title = title;
                this.simpleGlobalHotkeyService = UnityHandler.UnityContainer.Resolve<ISimpleGlobalHotkeyService>();

                this.simpleGlobalHotkeyService.ProcessingHotkeys = false;
                this.simpleGlobalHotkeyService.KeyEvent += SimpleGlobalHotkeyService_KeyEvent;

                this.Closing += HotkeyPromptWindow_Closing;
            }
        }

        public string HotkeysAppSetting
        {
            get;
            set;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void SimpleGlobalHotkeyService_KeyEvent(object sender, SimpleGlobalHotkeyServiceEventArgs e)
        {
            HotkeyDisplay.HotkeySettingString = e.AsSettingString;

            if (e.KeyDown)
            {
                if (!e.KeyIsModifier)
                {
                    HotkeysAppSetting = e.AsSettingString;
                    DialogResult = true;
                }
            }
        }

        private void HotkeyPromptWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.simpleGlobalHotkeyService.KeyEvent -= SimpleGlobalHotkeyService_KeyEvent;
            this.simpleGlobalHotkeyService.ProcessingHotkeys = true;
        }
    }
}