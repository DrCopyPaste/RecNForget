using RecNForget.WPF.Services.Contracts;
using System.ComponentModel;
using System.Windows;

namespace RecNForget.Services.Contracts
{
    public interface IApplicationHotkeyService
    {
        void PauseCapturingHotkeys(bool pause = true);

        void ResumeCapturingHotkeys();
        void ResetAndReadHotkeysFromConfig();
    }
}
