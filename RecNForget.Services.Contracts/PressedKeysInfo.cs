using System;

namespace PressingIssue.Services.Contracts
{
    public struct PressedKeysInfo
    {
        public Keys Keys { get; set; }
        public bool IsWinPressed { get; set; }
        public bool IsAltPressed { get; set; }
        public bool IsCtrlPressed { get; set; }
        public bool IsShiftPressed { get; set; }

        public static PressedKeysInfo Empty => new PressedKeysInfo() { Keys = Keys.None, IsAltPressed = false, IsCtrlPressed = false, IsShiftPressed = false, IsWinPressed = false };
        public static PressedKeysInfo FromString(string settingString)
        {
            var settingValues = settingString.Split(";");

            return new PressedKeysInfo()
            {
                Keys = (Keys)Enum.Parse(typeof(Keys), settingValues[0].Split("=")[1]),
                IsWinPressed = bool.Parse(settingValues[1].Split("=")[1]),
                IsAltPressed = bool.Parse(settingValues[2].Split("=")[1]),
                IsCtrlPressed = bool.Parse(settingValues[3].Split("=")[1]),
                IsShiftPressed = bool.Parse(settingValues[4].Split("=")[1])
            };
        }

        /// <summary>
        /// Generates a string matching a given PressedKeysInfo
        /// </summary>
        /// <param name="pressedKeysInfo"></param>
        /// <returns></returns>
        public override string ToString()
        {
            // cannot really determine here which keys were lifted (user might lift multiple keys at once, but we might not catch every one of them, but we should know which modifier is being pressed)
            string pressedKey = Enum.IsDefined(typeof(ModifierKeys), (int)this.Keys) ? "None" : this.Keys.ToString();

            // this settings format is to be compatible with previously used fmutils keyboard hook
            return string.Format("Key={0}; Win={1}; Alt={2}; Ctrl={3}; Shift={4}", new object[] { pressedKey, this.IsWinPressed, this.IsAltPressed, this.IsCtrlPressed, this.IsShiftPressed });
        }
    }
}
