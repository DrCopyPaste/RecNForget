using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl, INotifyPropertyChanged
    {
        private bool withSettingsButton = false;

        public bool WithSettingsButton
        {
            get { return withSettingsButton; }
            set { withSettingsButton = value; OnPropertyChanged(); }
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            App.ContextMenu.IsOpen = true;
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
    }
}
