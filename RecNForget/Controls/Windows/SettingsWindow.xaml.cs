using Microsoft.Win32;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

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
            Closing += SettingsWindow_Closing;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.hotkeyService = new DesignerApplicationHotkeyService();
                SettingService = new DesignerAppSettingService();
            }
            else
            {
                this.hotkeyService = hotkeyService;
                SettingService = settingService;
            }
        }

        private void SettingsWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
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
            var dialog = new OpenFolderDialog();
            if (!string.IsNullOrEmpty(SettingService.OutputPath))
            {
                dialog.DefaultDirectory = SettingService.OutputPath;
            };

            if (dialog.ShowDialog() == true)
            {
                SettingService.OutputPath = dialog.FolderName;
            }

            // since there are two buttons on top of each other
            e.Handled = true;
        }

        private void Configure_ExportOutputPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            if (!string.IsNullOrEmpty(SettingService.ExportOutputPath))
            {
                dialog.DefaultDirectory = SettingService.ExportOutputPath;
            };

            if (dialog.ShowDialog() == true)
            {
                SettingService.ExportOutputPath = dialog.FolderName;
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
