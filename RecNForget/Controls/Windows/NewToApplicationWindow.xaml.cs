using RecNForget.ViewModels;
using System.Windows;
using System.Windows.Input;

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
