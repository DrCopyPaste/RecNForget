namespace PressingIssue.Services.Contracts.Events
{
    public class SimpleGlobalHotkeyServiceEventArgs
    {
        public bool KeyDown { get; private set; }
        public bool KeyUp { get; private set; }
        public bool KeyIsModifier { get; private set; }
        public string Key { get; private set; }
        public string AsSettingString { get; private set; }
        public bool IsWinPressed { get; private set; }
        public bool IsAltPressed { get; private set; }
        public bool IsCtrlPressed { get; private set; }
        public bool IsShiftPressed { get; private set; }

        public SimpleGlobalHotkeyServiceEventArgs(bool keyDown, string key, bool keyIsModifier, bool isWinPressed, bool isAltPressed, bool isCtrlPressed, bool isShiftPressed, string asSettingString)
        {
            this.Key = key;

            this.KeyDown = keyDown;
            this.KeyUp = !keyDown;

            this.KeyIsModifier = keyIsModifier;

            this.IsWinPressed = isWinPressed;
            this.IsAltPressed = isAltPressed;
            this.IsCtrlPressed = isCtrlPressed;
            this.IsShiftPressed = isShiftPressed;

            this.AsSettingString = asSettingString;
        }
    }
}
