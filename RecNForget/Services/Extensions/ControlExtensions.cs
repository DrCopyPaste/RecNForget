using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Services.Extensions
{
    public static class ControlExtensions
    {
        // https://stackoverflow.com/a/7075718
        public static void PerformClick(this Button btn)
        {
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
