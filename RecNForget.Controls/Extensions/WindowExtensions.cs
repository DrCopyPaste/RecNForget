using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls.Extensions
{
    public static class WindowExtensions
    {
        // returns the top left point of window positioned relatively to ownerControl
        public static void SetViewablePositionFromOwner(this Window window, Control ownerControl = null)
        {

            // TODO: get a sensible position to show the window:
            // if ownerControl has value: center of window should be center of ownerControl (but ajusted conditionally to fit on screen)
            // if ownerControl has no value: center of window should be "in vision" (use primary screen?) and ajusted to fit on screen
            var returnValue = new Point(
                x: ownerControl != null ? Window.GetWindow(ownerControl).Left : window.Left,
                y: ownerControl != null ? Window.GetWindow(ownerControl).Top : window.Top);

            window.Left = returnValue.X;
            window.Top = returnValue.Y;
        }
    }
}
