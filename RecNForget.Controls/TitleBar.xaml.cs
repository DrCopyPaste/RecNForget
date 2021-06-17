﻿using RecNForget.IoC;
using RecNForget.WPF.Services.Contracts;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl, INotifyPropertyChanged
    {
        private IActionService actionService = null;

        private bool withSettingsButton = false;

        public bool WithSettingsButton
        {
            get { return withSettingsButton; }
            set { withSettingsButton = value; OnPropertyChanged(); }
        }

        private bool withWindowOptionsButton = false;

        public bool WithWindowOptionsButton
        {
            get { return withWindowOptionsButton; }
            set { withWindowOptionsButton = value; OnPropertyChanged(); }
        }

        private bool withMinimizeButton = false;

        public bool WithMinimizeButton
        {
            get { return withMinimizeButton; }
            set { withMinimizeButton = value; OnPropertyChanged(); }
        }

        private bool exitIsCancel = true;
        public bool ExitIsCancel
        {
            get { return exitIsCancel; }
            set { exitIsCancel = value; OnPropertyChanged(); }
        }


        public TitleBar()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.actionService = new RecNForget.Services.Designer.DesignerActionService();
                return;
            }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;
                this.actionService = UnityHandler.UnityContainer.Resolve<IActionService>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            actionService.ShowApplicationMenu();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            parentWindow.WindowState = WindowState.Minimized;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }

        private void WindowOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            actionService.ChangeTheme("Simple_White");

            //var rng = new Random();


            //string themeName = "Simple_White.xaml";

            //var name = rng.Next() % 2 == 0 ? "/RecNForget.Controls;component/Themes/Simple_Black.xaml" : "/RecNForget.Controls;component/Themes/Simple_White.xaml";

            ////Uri dictUri = new Uri(name, UriKind.RelativeOrAbsolute);

            ////Uri dictUri = new Uri(@"/Resources/Themes/MyTheme.xaml", UriKind.Relative);
            ////Uri dictUri = new Uri("pack://application:,,,/RecNForget.Controls;component/Themes/Simple_White.xaml", UriKind.RelativeOrAbsolute);

            ////ResourceDictionary resourceDict = Application.LoadComponent(dictUri) as ResourceDictionary;

            ////Application.Current.Resources.MergedDictionaries.Clear();
            ////Application.Current.Resources.MergedDictionaries[0] = resourceDict;
            ////Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            //try
            //{
            //    Application.Current.Resources.Source = new Uri("pack://application:,,,/RecNForget.Controls;component/Themes/" + themeName);
            //    Window.GetWindow(this).Resources.Source = new Uri("pack://application:,,,/RecNForget.Controls;component/Themes/" + themeName);
            //}
            //catch
            //{ }

            //Window.GetWindow(this).InvalidateVisual();
            //Window.GetWindow(this).Hide();
            //Window.GetWindow(this).Show();
        }
    }
}
