using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls.Extensions
{
    public static class ControlExtensions
    {
        // https://stackoverflow.com/a/7075718
        public static void PerformClick(this Button btn, bool ignoreIsEnabled = false)
        {
            if (ignoreIsEnabled || btn.IsEnabled)
            {
                btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
