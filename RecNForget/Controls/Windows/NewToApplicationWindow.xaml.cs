using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.WPF.Services.Contracts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using RecNForget.Services;
using RecNForget.ViewModels;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for NewToApplicationWindow.xaml
    /// </summary>
    public partial class NewToApplicationWindow : Window
    {
        public NewToApplicationWindow(NewToApplicationViewModel newToApplicationViewModel)
        {
            InitializeComponent();
            DataContext = newToApplicationViewModel;         
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
