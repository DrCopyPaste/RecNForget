using System;
using PressingIssue.Services.Contracts.Events;

namespace PressingIssue.Services.Contracts
{
    public interface IMultiKeyGlobalHotkeyService : IBasicGlobalHotkeyService
    {
        // you can attach to this to do additional actions on key down/up
        // but it is not needed for processing hotkeys
        event EventHandler<MultiKeyGlobalHotkeyServiceEventArgs> KeyEvent;
    }
}
