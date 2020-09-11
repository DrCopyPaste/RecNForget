using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace RecNForget.Services.Helpers
{
    public class HotkeySettingTranslator
    {
        public static List<Key> ModifierKeys
        {
            get
            {
                return new List<Key>()
                {
                    Key.LeftShift,
                    Key.RightShift,
                    Key.LeftCtrl,
                    Key.RightCtrl,
                    Key.LeftAlt,
                    Key.RightAlt,
                    Key.CapsLock,
                    Key.LWin,
                    Key.RWin
                };
            }
        }

        public static string GetHotkeySettingAsString(string settingKey, string keySeparator = " + ", string keyStart = "[", string keyEnd = "]")
        {
            return string.Join(keySeparator, GetHotkeySettingAsList(settingKey, keyStart, keyEnd));
        }

        //public static string GetKeyboardHookEventArgsAsString(KeyboardHookEventArgs keyboardHookEventArgs, string keySeparator = " + ", string keyStart = "[", string keyEnd = "]")
        //{
        //    return string.Join(keySeparator, GetKeyboardHookEventArgsAsList(keyboardHookEventArgs, keyStart, keyEnd));
        //}

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

        //public static List<string> GetKeyboardHookEventArgsAsList(KeyboardHookEventArgs keyboardHookEventArgs, string keyStart = "[", string keyEnd = "]")
        //{
        //    List<string> keys = new List<string>();

        //    // modifier keys
        //    if (keyboardHookEventArgs.isShiftPressed)
        //    {
        //        keys.Add(keyStart + "Shift" + keyEnd);
        //    }

        //    if (keyboardHookEventArgs.isCtrlPressed)
        //    {
        //        keys.Add(keyStart + "Ctrl" + keyEnd);
        //    }

        //    if (keyboardHookEventArgs.isAltPressed)
        //    {
        //        keys.Add(keyStart + "Alt" + keyEnd);
        //    }

        //    if (keyboardHookEventArgs.isWinPressed)
        //    {
        //        keys.Add(keyStart + "Win" + keyEnd);
        //    }

        //    // actual hotkey
        //    if (keyboardHookEventArgs.Key != Keys.None)
        //    {
        //        keys.Add(keyStart + keyboardHookEventArgs.Key.ToString() + keyEnd);
        //    }

        //    return keys;
        //}

        public static string GetKeyEventArgsAsString(System.Windows.Input.KeyEventArgs keyEventArgs, ModifierKeys modifiers, string keySeparator = " + ", string keyStart = "[", string keyEnd = "]")
        {
            return string.Join(keySeparator, GetKeyEventArgsAsList(keyEventArgs, modifiers, keyStart, keyEnd));
        }

        public static List<string> GetKeyEventArgsAsList(System.Windows.Input.KeyEventArgs keyEventArgs, ModifierKeys modifiers, string keyStart = "[", string keyEnd = "]")
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
            if (keyEventArgs.Key != Key.None && !ModifierKeys.Contains(keyEventArgs.Key))
            {
                keys.Add(keyStart + keyEventArgs.Key.ToString() + keyEnd);
            }

            return keys;
        }

        public static string GetSettingStringFromKeyGesture(KeyGesture gesture)
        {
            return string.Format("Key={0}; Win={1}; Alt={2}; Ctrl={3}; Shift={4}",
                gesture.Key.ToString(),
                (gesture.Modifiers & System.Windows.Input.ModifierKeys.Windows) > 0,
                (gesture.Modifiers & System.Windows.Input.ModifierKeys.Alt) > 0,
                (gesture.Modifiers & System.Windows.Input.ModifierKeys.Control) > 0,
                (gesture.Modifiers & System.Windows.Input.ModifierKeys.Shift) > 0);
        }
    }
}