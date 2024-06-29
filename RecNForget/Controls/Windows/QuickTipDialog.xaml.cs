using RecNForget.Help;
using RecNForget.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for QuickTipDialog.xaml
    /// </summary>
    public partial class QuickTipDialog : Window
    {
        private string featureCaption;
        private string featureContents;

        public QuickTipDialog(QuickTipViewModel quickTipViewModel)
        {
            InitializeComponent();
            DataContext = quickTipViewModel;

            this.KeyDown += Window_KeyDown;
        }

        public void SetQuickTip(HelpFeature helpFeature)
        {
            ((QuickTipViewModel)DataContext).SetFeatureCommand.Execute(helpFeature);
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
