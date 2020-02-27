using System.Windows;
using System.Windows.Input;

namespace RecNForget.Windows
{
    /// <summary>
    /// Interaction logic for CloseOrBackgroundDialog.xaml
    /// </summary>
    public partial class CloseOrBackgroundDialog : Window
    {
        public CloseOrBackgroundDialog()
        {
            InitializeComponent();

            this.Title = "Close or continue in background?";
        }

        private bool ContinueInBackground { get; set; }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ContinueInBackground = false;
            DialogResult = ContinueInBackground;
        }

        private void Background_Click(object sender, RoutedEventArgs e)
        {
            ContinueInBackground = true;
            DialogResult = ContinueInBackground;
        }
    }
}
