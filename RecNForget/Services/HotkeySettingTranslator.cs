using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using FMUtils.KeyboardHook;
using RecNForget.Services.Extensions;

namespace RecNForget.Services
{
    public class HotkeySettingTranslator
    {
        public static string GetHotkeySettingAsString(string settingKey, string keySeparator = " + ", string keyStart = "[", string keyEnd = "]")
        {
            return string.Join(keySeparator, GetHotkeySettingAsList(settingKey, keyStart, keyEnd));
        }

        public static string GetKeyboardHookEventArgsAsString(KeyboardHookEventArgs keyboardHookEventArgs, string keySeparator = " + ", string keyStart = "[", string keyEnd = "]")
        {
            return string.Join(keySeparator, GetKeyboardHookEventArgsAsList(keyboardHookEventArgs, keyStart, keyEnd));
        }

        public static List<string> GetHotkeySettingAsList(string setting, string keyStart = "[", string keyEnd = "]")
        {
            List<string> keys = new List<string>();

            // modifier keys
            if (setting.Contains("Shift=True"))
            {
                keys.Add(keyStart + "Shift" + keyEnd);
            }

            if (setting.Contains("Ctrl=True"))
            {
                keys.Add(keyStart + "Ctrl" + keyEnd);
            }

            if (setting.Contains("Alt=True"))
            {
                keys.Add(keyStart + "Alt" + keyEnd);
            }

            if (setting.Contains("Win=True"))
            {
                keys.Add(keyStart + "Win" + keyEnd);
            }

            var actualKey = setting.Split(';')[0].Replace("Key=", string.Empty);
            if (actualKey != string.Empty && actualKey != "None")
            {
                keys.Add(keyStart + actualKey + keyEnd);
            }

            return keys;
        }

        public static List<string> GetKeyboardHookEventArgsAsList(KeyboardHookEventArgs keyboardHookEventArgs, string keyStart = "[", string keyEnd = "]")
        {
            List<string> keys = new List<string>();

            // modifier keys
            if (keyboardHookEventArgs.isShiftPressed)
            {
                keys.Add(keyStart + "Shift" + keyEnd);
            }

            if (keyboardHookEventArgs.isCtrlPressed)
            {
                keys.Add(keyStart + "Ctrl" + keyEnd);
            }

            if (keyboardHookEventArgs.isAltPressed)
            {
                keys.Add(keyStart + "Alt" + keyEnd);
            }

            if (keyboardHookEventArgs.isWinPressed)
            {
                keys.Add(keyStart + "Win" + keyEnd);
            }

            // actual hotkey
            if (keyboardHookEventArgs.Key != Keys.None)
            {
                keys.Add(keyStart + keyboardHookEventArgs.Key.ToString() + keyEnd);
            }

            return keys;
        }

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
