using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Services.Extensions
{
    public static class GridExtensions
    {
        public static void InsertAt(this Grid grid, UIElement element, int column, int row)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            grid.Children.Add(element);
        }
    }
}
