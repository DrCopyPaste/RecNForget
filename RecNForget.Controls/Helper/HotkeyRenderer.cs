using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using RecNForget.Controls.Extensions;

namespace RecNForget.Controls.Helper
{
    public class HotkeyRenderer
    {
        public static Grid GetHotkeyListAsButtonGrid(List<string> hotkeys, Style buttonStyle = null, double? spacing = null, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Center)
        {
            int currentIndex = 0;
            var buttonGrid = new Grid();

            buttonGrid.HorizontalAlignment = horizontalAlignment;

            var rowDefinition = new RowDefinition();
            rowDefinition.Height = GridLength.Auto;
            buttonGrid.RowDefinitions.Add(rowDefinition);

            for (int i = 0; i < hotkeys.Count; i++)
            {
                var columnDefinition = new ColumnDefinition();
                columnDefinition.Width = GridLength.Auto;
                buttonGrid.ColumnDefinitions.Add(columnDefinition);

                if (spacing.HasValue && (i != hotkeys.Count - 1))
                {
                    var spacingDefinition = new ColumnDefinition();
                    spacingDefinition.Width = new GridLength(spacing.Value);
                    buttonGrid.ColumnDefinitions.Add(spacingDefinition);
                }
            }

            // if we have spacing enabled, indices after 0 will have to be multiplied two
            // before spacing: 0 1 2 3 4 (button indices)
            // after spacing: 0 s 1 s 2 s 3 s 4
            // after spacing: 0 s 2 s 4 s 6 s 8
            foreach (var hotkeyName in hotkeys)
            {
                var tempButton = new System.Windows.Controls.Button();
                if (buttonStyle != null)
                {
                    tempButton.Style = buttonStyle;
                }

                tempButton.Content = hotkeyName;
                buttonGrid.InsertAt(tempButton, spacing.HasValue ? currentIndex * 2 : currentIndex, 0);
                currentIndex++;
            }

            return buttonGrid;
        }
    }
}
