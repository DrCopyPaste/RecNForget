using System.ComponentModel;
using System.Windows;

namespace RecNForget.Services.Contracts
{
    public interface IApplicationHotkeyService
    {
        void ResetAndReadHotkeysFromConfig();

        void PauseCapturingHotkeys(bool pause = true);

        void ResumeCapturingHotkeys();
    }
}
