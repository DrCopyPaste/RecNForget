namespace RecNForget.Services.Contracts
{
    public interface IApplicationHotkeyService
    {
        void PauseCapturingHotkeys(bool pause = true);

        void ResumeCapturingHotkeys();
        void ResetAndReadHotkeysFromConfig();
    }
}
