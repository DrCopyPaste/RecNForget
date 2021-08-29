using System;
using PressingIssue.Services.Contracts.Events;

namespace PressingIssue.Services.Contracts
{
    public interface ISimpleGlobalHotkeyService : IBasicGlobalHotkeyService
    {
        // you can attach to this to do additional actions on key down/up
        // but it is not needed for processing hotkeys
        event EventHandler<SimpleGlobalHotkeyServiceEventArgs> KeyEvent;

        void AddOrUpdateQuickCastHotkey(PressedKeysInfo pressedKeysInfo, Action hotkeyAction);

        void AddOrUpdateOnReleaseHotkey(PressedKeysInfo pressedKeysInfo, Action hotkeyAction);

        string GetPressedKeysAsSetting(PressedKeysInfo pressedKeysInfo);
    }
}
