using Microsoft.Win32;
using RecNForget.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for NewToVersionDialog.xaml
    /// </summary>
    public partial class NewToVersionDialog : Window
    {
        public NewToVersionDialog(NewToVersionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            this.Title = "Things changed since we last met";
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
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
            this.Close();
        }
    }
}
