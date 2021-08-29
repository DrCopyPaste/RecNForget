using System;

namespace PressingIssue.Services.Contracts
{
    public interface IBasicGlobalHotkeyService : IDisposable
    {
        bool ProcessingHotkeys { get; set; }

        void Start(bool processingHotkeys = true);
        void Stop();

        void RemoveAllHotkeys();
    }
}
