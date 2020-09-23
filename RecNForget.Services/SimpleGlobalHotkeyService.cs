using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PressingIssue.Services.Contracts;
using PressingIssue.Services.Contracts.Events;

namespace PressingIssue.Services.Win32
{
    public class SimpleGlobalHotkeyService : ISimpleGlobalHotkeyService
    {
        #region pinvoke virtual key states

        // https://www.pinvoke.net/default.aspx/user32.getkeystate
        enum VirtualKeyStates : int
        {
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
            VK_CANCEL = 0x03,
            VK_MBUTTON = 0x04,
            //
            VK_XBUTTON1 = 0x05,
            VK_XBUTTON2 = 0x06,
            //
            VK_BACK = 0x08,
            VK_TAB = 0x09,
            //
            VK_CLEAR = 0x0C,
            VK_RETURN = 0x0D,
            //
            VK_SHIFT = 0x10,
            VK_CONTROL = 0x11,
            VK_MENU = 0x12,
            VK_PAUSE = 0x13,
            VK_CAPITAL = 0x14,
            //
            VK_KANA = 0x15,
            VK_HANGEUL = 0x15,  /* old name - should be here for compatibility */
            VK_HANGUL = 0x15,
            VK_JUNJA = 0x17,
            VK_FINAL = 0x18,
            VK_HANJA = 0x19,
            VK_KANJI = 0x19,
            //
            VK_ESCAPE = 0x1B,
            //
            VK_CONVERT = 0x1C,
            VK_NONCONVERT = 0x1D,
            VK_ACCEPT = 0x1E,
            VK_MODECHANGE = 0x1F,
            //
            VK_SPACE = 0x20,
            VK_PRIOR = 0x21,
            VK_NEXT = 0x22,
            VK_END = 0x23,
            VK_HOME = 0x24,
            VK_LEFT = 0x25,
            VK_UP = 0x26,
            VK_RIGHT = 0x27,
            VK_DOWN = 0x28,
            VK_SELECT = 0x29,
            VK_PRINT = 0x2A,
            VK_EXECUTE = 0x2B,
            VK_SNAPSHOT = 0x2C,
            VK_INSERT = 0x2D,
            VK_DELETE = 0x2E,
            VK_HELP = 0x2F,
            //
            VK_LWIN = 0x5B,
            VK_RWIN = 0x5C,
            VK_APPS = 0x5D,
            //
            VK_SLEEP = 0x5F,
            //
            VK_NUMPAD0 = 0x60,
            VK_NUMPAD1 = 0x61,
            VK_NUMPAD2 = 0x62,
            VK_NUMPAD3 = 0x63,
            VK_NUMPAD4 = 0x64,
            VK_NUMPAD5 = 0x65,
            VK_NUMPAD6 = 0x66,
            VK_NUMPAD7 = 0x67,
            VK_NUMPAD8 = 0x68,
            VK_NUMPAD9 = 0x69,
            VK_MULTIPLY = 0x6A,
            VK_ADD = 0x6B,
            VK_SEPARATOR = 0x6C,
            VK_SUBTRACT = 0x6D,
            VK_DECIMAL = 0x6E,
            VK_DIVIDE = 0x6F,
            VK_F1 = 0x70,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 0x78,
            VK_F10 = 0x79,
            VK_F11 = 0x7A,
            VK_F12 = 0x7B,
            VK_F13 = 0x7C,
            VK_F14 = 0x7D,
            VK_F15 = 0x7E,
            VK_F16 = 0x7F,
            VK_F17 = 0x80,
            VK_F18 = 0x81,
            VK_F19 = 0x82,
            VK_F20 = 0x83,
            VK_F21 = 0x84,
            VK_F22 = 0x85,
            VK_F23 = 0x86,
            VK_F24 = 0x87,
            //
            VK_NUMLOCK = 0x90,
            VK_SCROLL = 0x91,
            //
            VK_OEM_NEC_EQUAL = 0x92,   // '=' key on numpad
                                       //
            VK_OEM_FJ_JISHO = 0x92,   // 'Dictionary' key
            VK_OEM_FJ_MASSHOU = 0x93,   // 'Unregister word' key
            VK_OEM_FJ_TOUROKU = 0x94,   // 'Register word' key
            VK_OEM_FJ_LOYA = 0x95,   // 'Left OYAYUBI' key
            VK_OEM_FJ_ROYA = 0x96,   // 'Right OYAYUBI' key
                                     //
            VK_LSHIFT = 0xA0,
            VK_RSHIFT = 0xA1,
            VK_LCONTROL = 0xA2,
            VK_RCONTROL = 0xA3,
            VK_LMENU = 0xA4,
            VK_RMENU = 0xA5,
            //
            VK_BROWSER_BACK = 0xA6,
            VK_BROWSER_FORWARD = 0xA7,
            VK_BROWSER_REFRESH = 0xA8,
            VK_BROWSER_STOP = 0xA9,
            VK_BROWSER_SEARCH = 0xAA,
            VK_BROWSER_FAVORITES = 0xAB,
            VK_BROWSER_HOME = 0xAC,
            //
            VK_VOLUME_MUTE = 0xAD,
            VK_VOLUME_DOWN = 0xAE,
            VK_VOLUME_UP = 0xAF,
            VK_MEDIA_NEXT_TRACK = 0xB0,
            VK_MEDIA_PREV_TRACK = 0xB1,
            VK_MEDIA_STOP = 0xB2,
            VK_MEDIA_PLAY_PAUSE = 0xB3,
            VK_LAUNCH_MAIL = 0xB4,
            VK_LAUNCH_MEDIA_SELECT = 0xB5,
            VK_LAUNCH_APP1 = 0xB6,
            VK_LAUNCH_APP2 = 0xB7,
            //
            VK_OEM_1 = 0xBA,   // ';:' for US
            VK_OEM_PLUS = 0xBB,   // '+' any country
            VK_OEM_COMMA = 0xBC,   // ',' any country
            VK_OEM_MINUS = 0xBD,   // '-' any country
            VK_OEM_PERIOD = 0xBE,   // '.' any country
            VK_OEM_2 = 0xBF,   // '/?' for US
            VK_OEM_3 = 0xC0,   // '`~' for US
                               //
            VK_OEM_4 = 0xDB,  //  '[{' for US
            VK_OEM_5 = 0xDC,  //  '\|' for US
            VK_OEM_6 = 0xDD,  //  ']}' for US
            VK_OEM_7 = 0xDE,  //  ''"' for US
            VK_OEM_8 = 0xDF,
            //
            VK_OEM_AX = 0xE1,  //  'AX' key on Japanese AX kbd
            VK_OEM_102 = 0xE2,  //  "<>" or "\|" on RT 102-key kbd.
            VK_ICO_HELP = 0xE3,  //  Help key on ICO
            VK_ICO_00 = 0xE4,  //  00 key on ICO
                               //
            VK_PROCESSKEY = 0xE5,
            //
            VK_ICO_CLEAR = 0xE6,
            //
            VK_PACKET = 0xE7,
            //
            VK_OEM_RESET = 0xE9,
            VK_OEM_JUMP = 0xEA,
            VK_OEM_PA1 = 0xEB,
            VK_OEM_PA2 = 0xEC,
            VK_OEM_PA3 = 0xED,
            VK_OEM_WSCTRL = 0xEE,
            VK_OEM_CUSEL = 0xEF,
            VK_OEM_ATTN = 0xF0,
            VK_OEM_FINISH = 0xF1,
            VK_OEM_COPY = 0xF2,
            VK_OEM_AUTO = 0xF3,
            VK_OEM_ENLW = 0xF4,
            VK_OEM_BACKTAB = 0xF5,
            //
            VK_ATTN = 0xF6,
            VK_CRSEL = 0xF7,
            VK_EXSEL = 0xF8,
            VK_EREOF = 0xF9,
            VK_PLAY = 0xFA,
            VK_ZOOM = 0xFB,
            VK_NONAME = 0xFC,
            VK_PA1 = 0xFD,
            VK_OEM_CLEAR = 0xFE
        }

        #endregion

        #region pinvoke

        private const int KEY_PRESSED = 0x8000;

        // http://pinvoke.net/default.aspx/user32/GetAsyncKeyState.html
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int nVirtKey);

        // https://www.pinvoke.net/default.aspx/user32.getkeystate
        [DllImport("user32.dll")]
        static extern short GetKeyState(VirtualKeyStates nVirtKey);

        #endregion

        private readonly GlobalKeyboardHook keyboardHook = null;
        private readonly HashSet<string> modifierKeys = null;

        // key = hotkeysettingstring, value = currently held down
        private readonly Dictionary<string, bool> hotkeyPressedStates = null;
        private readonly Dictionary<string, Action> quickCastHotkeys = null;
        private readonly Dictionary<string, Action> onReleaseHotkeys = null;


        public event EventHandler<SimpleGlobalHotkeyServiceEventArgs> KeyEvent;
        public bool Running { get; private set; } = false;
        public bool ProcessingHotkeys { get; set; } = true;

        public SimpleGlobalHotkeyService()
        {
            modifierKeys = new HashSet<string>()
            {
                Keys.Control.ToString(),
                Keys.LControlKey.ToString(),
                Keys.RControlKey.ToString(),
                Keys.LWin.ToString(),
                Keys.RWin.ToString(),
                Keys.Alt.ToString(),
                Keys.LMenu.ToString(),
                Keys.RMenu.ToString(),
                Keys.RShiftKey.ToString(),
                Keys.LShiftKey.ToString(),
            };

            keyboardHook = new GlobalKeyboardHook();

            hotkeyPressedStates = new Dictionary<string, bool>();
            quickCastHotkeys = new Dictionary<string, Action>();
            onReleaseHotkeys = new Dictionary<string, Action>();

            Start();
        }

        private void KeyboardHookEvent(object sender, GlobalKeyboardHook.GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyDown)
            {
                var pressedKeysAsConfig = GetPressedKeysAsSetting(e);

                if (ProcessingHotkeys)
                {
                    ProcessHotkeysDown(pressedKeysAsConfig);
                }

                KeyChangedEvent(e, pressedKeysAsConfig);
            }
            else if (e.KeyUp)
            {
                if (ProcessingHotkeys)
                {
                    ProcessHotkeysUp();
                }

                // ensure hotkeys are not pressed even if not in ProcessingHotkeys mode
                ResetHotkeyPressedStates();
                var pressedKeysAsConfig = GetPressedKeysAsSetting(e);

                KeyChangedEvent(e, pressedKeysAsConfig);
            }
        }

        private void KeyChangedEvent(GlobalKeyboardHook.GlobalKeyboardHookEventArgs e, string pressedKeysAsConfig)
        {
            try
            {
                KeyEvent?.Invoke(this, new SimpleGlobalHotkeyServiceEventArgs(e.KeyDown, e.KeyName, pressedKeysAsConfig));
            }
            catch
            {
                // silently ignore any errors when triggering events
            }
        }

        private string GetPressedKeysAsSetting(GlobalKeyboardHook.GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyUp)
            {
                // cannot really determine here which keys were lifted
                return string.Format("Key={0}; Win={1}; Alt={2}; Ctrl={3}; Shift={4}", new object[] { "None", false, false, false, false });
            }

            string pressedKey = modifierKeys.Contains(e.KeyName) ? "None" : e.KeyName;

            bool isWinPressed = Convert.ToBoolean(GetAsyncKeyState((int)VirtualKeyStates.VK_LWIN) & KEY_PRESSED) || Convert.ToBoolean(GetAsyncKeyState((int)VirtualKeyStates.VK_RWIN) & KEY_PRESSED) || (e.KeyDown && (e.KeyName == Keys.LWin.ToString() || e.KeyName == Keys.RWin.ToString()));
            bool isAltPressed = Convert.ToBoolean(GetAsyncKeyState((int)VirtualKeyStates.VK_LMENU) & KEY_PRESSED) || Convert.ToBoolean(GetAsyncKeyState((int)VirtualKeyStates.VK_RMENU) & KEY_PRESSED) || (e.KeyDown && (e.KeyName == Keys.LMenu.ToString() || e.KeyName == Keys.RMenu.ToString()));
            bool isCtrlPressed = Convert.ToBoolean(GetAsyncKeyState((int)VirtualKeyStates.VK_LCONTROL) & KEY_PRESSED) || Convert.ToBoolean(GetAsyncKeyState((int)VirtualKeyStates.VK_RCONTROL) & KEY_PRESSED) || (e.KeyDown && (e.KeyName == Keys.LControlKey.ToString() || e.KeyName == Keys.RControlKey.ToString()));
            bool isShiftPressed = Convert.ToBoolean(GetAsyncKeyState((int)VirtualKeyStates.VK_LSHIFT) & KEY_PRESSED) || Convert.ToBoolean(GetAsyncKeyState((int)VirtualKeyStates.VK_RSHIFT) & KEY_PRESSED) || (e.KeyDown && (e.KeyName == Keys.LShiftKey.ToString() || e.KeyName == Keys.RShiftKey.ToString()));

            // this settings format is to be compatible with previously used fmutils keyboard hook
            return string.Format("Key={0}; Win={1}; Alt={2}; Ctrl={3}; Shift={4}", new object[] { pressedKey, isWinPressed, isAltPressed, isCtrlPressed, isShiftPressed });
        }

        private void AddOrUpdateHotkeyState(string settingString)
        {
            if (this.hotkeyPressedStates.ContainsKey(settingString))
            {
                this.hotkeyPressedStates[settingString] = false;
            }
            else
            {
                this.hotkeyPressedStates.Add(settingString, false);
            }
        }

        private void ResetHotkeyPressedStates()
        {
            foreach (var keyName in hotkeyPressedStates.Keys.ToList())
            {
                hotkeyPressedStates[keyName] = false;
            }
        }

        private void ProcessHotkeysDown(string pressedKeysAsConfig)
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            if (hotkeyPressedStates.ContainsKey(pressedKeysAsConfig))
            {
                var couldTriggerQuickCast = !hotkeyPressedStates[pressedKeysAsConfig];
                hotkeyPressedStates[pressedKeysAsConfig] = true;

                if (couldTriggerQuickCast && quickCastHotkeys.Any() && quickCastHotkeys.ContainsKey(pressedKeysAsConfig))
                {
                    try
                    {
                        quickCastHotkeys[pressedKeysAsConfig].Invoke();
                    }
                    catch
                    {
                        // silently ignore any errors when triggering events
                    }
                }
            }
            else
            {
                ResetHotkeyPressedStates();
            }
        }

        private void ProcessHotkeysUp()
        {
            if (onReleaseHotkeys.Any())
            {
                var hotkeysWaitingForRelease = onReleaseHotkeys.Where(h => hotkeyPressedStates.ContainsKey(h.Key) && hotkeyPressedStates[h.Key]);

                if (hotkeysWaitingForRelease.Any())
                {
                    // only one action per hotkey allowed atm (dictionary), but that may change
                    foreach (var hotkeyAction in hotkeysWaitingForRelease)
                    {
                        try
                        {
                            hotkeyAction.Value.Invoke();
                        }
                        catch
                        {
                            // silently ignore any errors when triggering events
                        }
                    }
                }
            }
        }

        public void Start(bool processHotkeys = true)
        {
            ProcessingHotkeys = processHotkeys;
            keyboardHook.KeyEvent += KeyboardHookEvent;
            keyboardHook.Start();
            Running = true;
        }

        public void Stop()
        {
            keyboardHook.Stop();
            keyboardHook.KeyEvent -= KeyboardHookEvent;
            Running = false;
        }

        public void AddOrUpdateQuickCastHotkey(string settingString, Action hotkeyAction)
        {
            AddOrUpdateHotkeyState(settingString);

            if (this.quickCastHotkeys.ContainsKey(settingString))
            {
                this.quickCastHotkeys[settingString] = hotkeyAction;
            }
            else
            {
                this.quickCastHotkeys.Add(settingString, hotkeyAction);
            }
        }

        public void AddOrUpdateOnReleaseHotkey(string settingString, Action hotkeyAction)
        {
            AddOrUpdateHotkeyState(settingString);

            if (this.onReleaseHotkeys.ContainsKey(settingString))
            {
                this.onReleaseHotkeys[settingString] = hotkeyAction;
            }
            else
            {
                this.onReleaseHotkeys.Add(settingString, hotkeyAction);
            }
        }

        public void RemoveAllHotkeys()
        {
            this.quickCastHotkeys.Clear();
            this.onReleaseHotkeys.Clear();
            this.hotkeyPressedStates.Clear();
        }

        public void Dispose()
        {
            Stop();
        }
    }


}
