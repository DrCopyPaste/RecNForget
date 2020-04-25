using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Ookii.Dialogs.Wpf;
using RecNForget.Controls.Helper;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.Services.Helpers;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : INotifyPropertyChanged
    {
        private IHotkeyService hotkeyService;
        private IAppSettingService settingService;

        public SettingsWindow(IHotkeyService hotkeyService, IAppSettingService settingService)
        {
            DataContext = this;
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.hotkeyService = new DesignerHotkeyService();
                SettingService = new DesignerAppSettingService();
            }
            else
            {
                this.hotkeyService = hotkeyService;
                SettingService = settingService;

                DisplayHotkey();
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

        private void DisplayHotkey()
        {
            if (HotkeyDisplay.Children.Count > 0)
            {
                HotkeyDisplay.Children.Clear();
            }

            var buttonGrid = HotkeyRenderer.GetHotkeyListAsButtonGrid(
                hotkeys: HotkeySettingTranslator.GetHotkeySettingAsList(SettingService.HotKey_StartStopRecording, string.Empty, string.Empty),
                buttonStyle: (Style)FindResource("HotkeyDisplayButton"),
                spacing: 6,
                horizontalAlignment: HorizontalAlignment.Left);

            HotkeyDisplay.Children.Add(buttonGrid);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ConfigureHotkey_StartStopRecording_Click(object sender, RoutedEventArgs e)
        {
            this.hotkeyService.PauseCapturingHotkeys();

            var dialog = new HotkeyPromptWindow("Configure start/stop recording hotkey");

            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                SettingService.HotKey_StartStopRecording = dialog.HotkeysAppSetting;
                DisplayHotkey();
            }

            this.hotkeyService.ResumeCapturingHotkeys();

            // since there are two buttons on top of each other
            e.Handled = true;
        }

        private void Configure_OutputPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();

            if (dialog.ShowDialog() == true)
            {
                SettingService.OutputPath = dialog.SelectedPath;
            }

            // since there are two buttons on top of each other
            e.Handled = true;
        }

        private void Configure_FileNamePattern_Click(object sender, RoutedEventArgs e)
        {
            CustomMessageBox tempDialog = new CustomMessageBox(
                caption: "Type in a new pattern for file name generation.",
                icon: CustomMessageBoxIcon.Question,
                buttons: CustomMessageBoxButtons.OkAndCancel,
                messageRows: new List<string>() { "Supported placeholders:", "(Date)" },
                prompt: SettingService.FilenamePrefix,
                controlFocus: CustomMessageBoxFocus.Prompt,
                promptValidationMode: CustomMessageBoxPromptValidation.EraseIllegalPathCharacters);

            if (tempDialog.ShowDialog().HasValue && tempDialog.Ok)
            {
                SettingService.FilenamePrefix = tempDialog.PromptContent;
            }

            // since there are two buttons on top of each other
            e.Handled = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
