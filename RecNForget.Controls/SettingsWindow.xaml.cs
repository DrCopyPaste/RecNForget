using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Ookii.Dialogs.Wpf;
using RecNForget.Controls.Helper;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : INotifyPropertyChanged
    {
        private IApplicationHotkeyService hotkeyService;
        private IAppSettingService settingService;

        public SettingsWindow(IApplicationHotkeyService hotkeyService, IAppSettingService settingService)
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.hotkeyService = new DesignerApplicationHotkeyService();
                SettingService = new DesignerAppSettingService();
            }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;
                this.hotkeyService = hotkeyService;
                SettingService = settingService;
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
            var dialog = new HotkeyPromptWindow("Configure start/stop recording hotkey");

            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                SettingService.HotKey_StartStopRecording = dialog.HotkeysAppSetting;
                this.hotkeyService.ResetAndReadHotkeysFromConfig();
            }

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
