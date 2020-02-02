using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Services.Extensions
{
    public static class FrameworkElementExtensions
    {
        public static void SetCustomToolTip(this FrameworkElement element, string text)
        {
            // https://www.telerik.com/forums/how-to-change-tooltip-duration or https://stackoverflow.com/a/8640109 ? either way not too beautiful
            element.ToolTip = text;
            ToolTipService.SetToolTip(element, element.ToolTip);
            ToolTipService.SetShowDuration(element, int.MaxValue);
        }
    }
}
