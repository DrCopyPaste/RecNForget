using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls.Extensions
{
    public static class GridExtensions
    {
        public static void InsertAt(this Grid grid, UIElement element, int columnX, int rowY)
        {
            Grid.SetRow(element, rowY);
            Grid.SetColumn(element, columnX);
            grid.Children.Add(element);
        }
    }
}
