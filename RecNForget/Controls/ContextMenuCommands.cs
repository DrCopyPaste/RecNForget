using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RecNForget.Controls.Helper;
using RecNForget.Controls.IoC;
using RecNForget.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls;

public partial class ContextMenuCommands : ObservableObject
{
    private readonly AboutViewModel aboutViewModel;
    private readonly SettingsViewModel settingsViewModel;

    public ContextMenuCommands(
        AboutViewModel aboutViewModel,
        SettingsViewModel settingsViewModel)
    {
        this.aboutViewModel = aboutViewModel;
        this.settingsViewModel = settingsViewModel;
    }

    private ContextMenu contextMenu;
    public ContextMenu ContextMenu
    {
        get
        {
            if (contextMenu != null) return contextMenu;

            contextMenu = new ContextMenu();
            //var themeItem = new MenuItem { Header = "Theme" };
            //var currentThemeName = settingService.WindowTheme.ToUpper();

            //foreach (var theme in ThemeManager.GetAllThemeNames())
            //{
            //    themeItem.Items.Add(new MenuItem
            //    {
            //        Header = theme.Value,
            //        Command = SetAppThemeCommand,
            //        CommandParameter = theme.Key,
            //        IsCheckable = true,
            //        IsChecked = currentThemeName == theme.Key
            //    });
            //}

            //contextMenu.Items.Add(themeItem);

            //contextMenu.Items.Add(new Separator());

            contextMenu.Items.Add(new MenuItem
            {
                Header = "always on top",
                Command = ToggleAlwaysOnTopCommand,
                IsCheckable = true,
                IsChecked = WindowAlwaysOnTop
            });

            contextMenu.Items.Add(new MenuItem
            {
                Header = "run in background",
                Command = ToggleBackgroundModeCommand,
                IsCheckable = true,
                IsChecked = MinimizedToTray
            });

            contextMenu.Items.Add(new Separator());

            contextMenu.Items.Add(new MenuItem
            {
                Header = "Show Output Path Control",
                Command = ToggleOutputPathControlCommand,
                IsCheckable = true,
                IsChecked = settingsViewModel.OutputPathControlVisible
            });
            contextMenu.Items.Add(new MenuItem
            {
                Header = "Show Selected File Control",
                Command = ToggleSelectedFileControlCommand,
                IsCheckable = true,
                IsChecked = settingsViewModel.SelectedFileControlVisible
            });
            contextMenu.Items.Add(new MenuItem
            {
                Header = "Show Recording Timer Control",
                Command = ToggleRecordingTimerControlCommand,
                IsCheckable = true,
                IsChecked = settingsViewModel.RecordingTimerControlVisible
            });

            contextMenu.Items.Add(new Separator());

            contextMenu.Items.Add(new MenuItem
            {
                Header = "Open settings",
                Command = ShowSettingsMenuCommand
            });

            contextMenu.Items.Add(new Separator());

            var helpItem = new MenuItem { Header = "Help" };
            helpItem.Items.Add(new MenuItem { Header = "About RecNFoget", Command = ShowAboutCommand });
            helpItem.Items.Add(new MenuItem { Header = "Show Help", Command = ShowHelpCommand });
            helpItem.Items.Add(new MenuItem { Header = "Check for Updates", Command = CheckForUpdatesCommand });

            contextMenu.Items.Add(helpItem);

            contextMenu.Items.Add(new Separator());

            contextMenu.Items.Add(new MenuItem { Header = "Exit", Command = ExitCommand });

            return contextMenu;
        }
    }

    [RelayCommand]
    private void SetAppTheme(string themeName)
    {
        ThemeManager.ChangeTheme(themeName);
    }

    [RelayCommand]
    private void ToggleAlwaysOnTop()
    {
        WindowAlwaysOnTop = !WindowAlwaysOnTop;
    }

    [RelayCommand]
    private void ToggleBackgroundMode()
    {
        MinimizedToTray = !MinimizedToTray;
    }

    [RelayCommand]
    private void ToggleOutputPathControl()
    {
        settingsViewModel.OutputPathControlVisible = !settingsViewModel.OutputPathControlVisible;
    }

    [RelayCommand]
    private void ToggleSelectedFileControl()
    {
        settingsViewModel.SelectedFileControlVisible = !settingsViewModel.SelectedFileControlVisible;
    }

    [RelayCommand]
    private void ToggleRecordingTimerControl()
    {
        settingsViewModel.RecordingTimerControlVisible = !settingsViewModel.RecordingTimerControlVisible;
    }

    [RelayCommand]
    private void ShowSettingsMenu()
    {
        var settingsWindow = ConfiguredServices.ServiceProvider.GetRequiredService<SettingsWindow>();
        settingsWindow.ShowDialog();
    }

    [RelayCommand]
    private void ShowAbout()
    {
        var aboutWindow = ConfiguredServices.ServiceProvider.GetRequiredService<AboutWindow>();
        aboutWindow.Show();
    }

    [RelayCommand]
    private void ShowHelp()
    {
        var helpWindow = ConfiguredServices.ServiceProvider.GetRequiredService<HelpWindow>();
        helpWindow.Show();
    }

    [RelayCommand]
    private void CheckForUpdates()
    {
        aboutViewModel.CheckForUpdatesCommand.Execute(true);
    }

    [RelayCommand]
    private void Exit()
    {
        Application.Current.Shutdown();
    }

    public bool WindowAlwaysOnTop
    {
        get => settingsViewModel.WindowAlwaysOnTop;
        set
        {
            SetProperty(settingsViewModel.WindowAlwaysOnTop, value, settingsViewModel, (x, y) => x.WindowAlwaysOnTop = y);
        }
    }

    public bool MinimizedToTray
    {
        get => settingsViewModel.MinimizedToTray;
        set
        {
            SetProperty(settingsViewModel.MinimizedToTray, value, settingsViewModel, (x, y) => x.MinimizedToTray = y);
        }
    }
}
