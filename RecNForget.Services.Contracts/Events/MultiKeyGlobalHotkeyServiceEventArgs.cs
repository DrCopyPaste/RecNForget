using System.Collections.Generic;

namespace PressingIssue.Services.Contracts.Events
{
    public class MultiKeyGlobalHotkeyServiceEventArgs
    {
        public bool KeyDown { get; private set; }
        public bool KeyUp { get; private set; }

        public List<string> PressedKeys { get; private set; }
        public HashSet<string> PressedNonModifierKeys { get; private set; }

        public string AsSettingString { get; private set; }

        public MultiKeyGlobalHotkeyServiceEventArgs(bool keyDown, List<string> pressedKeys, HashSet<string> pressedNonModifierKeys, string settingString)
        {
            this.KeyDown = keyDown;
            this.KeyUp = !keyDown;

            this.PressedKeys = pressedKeys;
            this.PressedNonModifierKeys = pressedNonModifierKeys;

            this.AsSettingString = settingString;
        }
    }
}
