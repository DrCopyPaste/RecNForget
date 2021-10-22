namespace PressingIssue.Services.Contracts.Events
{
    public class SimpleGlobalHotkeyServiceEventArgs
    {
        public bool KeyDown { get; private set; }
        public bool KeyUp { get; private set; }
        public Keys Key { get; private set; }
        public string AsSettingString { get; private set; }
        public bool IsWinPressed { get; private set; }
        public bool IsAltPressed { get; private set; }
        public bool IsCtrlPressed { get; private set; }
        public bool IsShiftPressed { get; private set; }

        public SimpleGlobalHotkeyServiceEventArgs(bool keyDown, PressedKeysInfo pressedKeysInfo)
        {
            this.Key = pressedKeysInfo.Keys;

            this.KeyDown = keyDown;
            this.KeyUp = !keyDown;

            this.IsWinPressed = pressedKeysInfo.IsWinPressed;
            this.IsAltPressed = pressedKeysInfo.IsAltPressed;
            this.IsCtrlPressed = pressedKeysInfo.IsCtrlPressed;
            this.IsShiftPressed = pressedKeysInfo.IsShiftPressed;
        }
    }
}
