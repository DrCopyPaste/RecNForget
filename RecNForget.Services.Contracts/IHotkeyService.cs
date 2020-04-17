using System.ComponentModel;
using System.Windows;

namespace RecNForget.Services.Contracts
{
    public interface IHotkeyService
    {
        void ResetAndReadHotkeysFromConfig();

        void PauseCapturingHotkeys(bool pause = true);

        void ResumeCapturingHotkeys();
    }
}
