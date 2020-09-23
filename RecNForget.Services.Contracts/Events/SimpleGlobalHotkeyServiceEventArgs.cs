namespace PressingIssue.Services.Contracts.Events
{
    public class SimpleGlobalHotkeyServiceEventArgs
    {
        public bool KeyDown { get; private set; }
        public bool KeyUp { get; private set; }

        public string Key { get; private set; }
        public string AsSettingString { get; private set; }

        public SimpleGlobalHotkeyServiceEventArgs(bool keyDown, string key, string settingString)
        {
            this.Key = key;

            this.KeyDown = keyDown;
            this.KeyUp = !keyDown;

            this.AsSettingString = settingString;
        }
    }
}
