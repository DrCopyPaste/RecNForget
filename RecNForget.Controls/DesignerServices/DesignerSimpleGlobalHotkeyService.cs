using System;
using PressingIssue.Services.Contracts;
using PressingIssue.Services.Contracts.Events;

namespace RecNForget.Services.Designer
{
    public class DesignerSimpleGlobalHotkeyService : ISimpleGlobalHotkeyService
    {
        public bool ProcessingHotkeys { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Running => throw new NotImplementedException();

        public event EventHandler<SimpleGlobalHotkeyServiceEventArgs> KeyEvent;

        public void AddOrUpdateOnReleaseHotkey(string settingString, Action hotkeyAction)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdateOnReleaseHotkey(PressedKeysInfo pressedKeysInfo, Action hotkeyAction)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdateQuickCastHotkey(string settingString, Action hotkeyAction)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdateQuickCastHotkey(PressedKeysInfo pressedKeysInfo, Action hotkeyAction)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string GetPressedKeysAsSetting(PressedKeysInfo pressedKeysInfo)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllHotkeys()
        {
            throw new NotImplementedException();
        }

        public void Start(bool processingHotkeys = true)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
