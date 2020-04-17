using System.Collections.Generic;
using System.Windows.Forms;
using FMUtils.KeyboardHook;

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
    }
}
