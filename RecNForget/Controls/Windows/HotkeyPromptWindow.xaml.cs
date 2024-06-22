using Microsoft.Extensions.DependencyInjection;
using PressingIssue.Services.Contracts;
using PressingIssue.Services.Contracts.Events;
using RecNForget.Controls.IoC;
using RecNForget.Services.Designer;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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
                this.simpleGlobalHotkeyService = ConfiguredServices.ServiceProvider.GetRequiredService<ISimpleGlobalHotkeyService>();

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
            var keysAsSettingString = simpleGlobalHotkeyService.GetPressedKeysAsSetting(e.PressedKeysInfo);
            HotkeyDisplay.HotkeySettingString = keysAsSettingString;

            if (e.KeyDown)
            {
                if (!Enum.IsDefined(typeof(PressingIssue.Services.Contracts.ModifierKeys), (int)e.Key))
                {
                    HotkeysAppSetting = keysAsSettingString;
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