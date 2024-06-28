using Microsoft.Win32;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private IApplicationHotkeyService hotkeyService;
        private IAppSettingService settingService;

        public SettingsWindow(SettingsViewModel settingsViewModel, IApplicationHotkeyService hotkeyService)
        {
            InitializeComponent();
            DataContext = settingsViewModel;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.hotkeyService = new DesignerApplicationHotkeyService();
            }
            else
            {
                this.hotkeyService = hotkeyService;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Configure_OutputPath_Click(object sender, RoutedEventArgs e)
        {
            ((SettingsViewModel)DataContext).Configure_OutputPath_ClickCommand.Execute(this);
        }

        private void Configure_ExportOutputPath_Click(object sender, RoutedEventArgs e)
        {
            ((SettingsViewModel)DataContext).Configure_ExportOutputPath_ClickCommand.Execute(this);
        }

        private void Configure_FileNamePattern_Click(object sender, RoutedEventArgs e)
        {
            ((SettingsViewModel)DataContext).Configure_FileNamePattern_ClickCommand.Execute(this);
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
