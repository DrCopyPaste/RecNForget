using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PressingIssue.Services.Contracts.Events;
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

        public static List<string> GetKeyEventArgsAsList(SimpleGlobalHotkeyServiceEventArgs keyEventArgs, ModifierKeys modifiers, string keyStart = "[", string keyEnd = "]")
        {
            List<string> keys = new List<string>();

            // modifier keys
            if ((modifiers & System.Windows.Input.ModifierKeys.Shift) > 0)
            {
                keys.Add(keyStart + "Shift" + keyEnd);
            }

            if ((modifiers & System.Windows.Input.ModifierKeys.Control) > 0)
            {
                keys.Add(keyStart + "Ctrl" + keyEnd);
            }

            if ((modifiers & System.Windows.Input.ModifierKeys.Alt) > 0)
            {
                keys.Add(keyStart + "Alt" + keyEnd);
            }

            if ((modifiers & System.Windows.Input.ModifierKeys.Windows) > 0)
            {
                keys.Add(keyStart + "Win" + keyEnd);
            }

            // actual hotkey
            if (keyEventArgs.Key != "None")
            {
                keys.Add(keyStart + keyEventArgs.Key.ToString() + keyEnd);
            }

            return keys;
        }
    }
}
