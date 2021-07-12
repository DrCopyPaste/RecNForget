using System.Threading.Tasks;
using System.Windows.Threading;
using PressingIssue.Services.Contracts;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;

namespace RecNForget.Services
{
    public class ApplicationHotkeyService : IApplicationHotkeyService
    {
        private readonly IAppSettingService appSettingService;
        private readonly IAudioRecordingService audioRecordingService;
        private readonly IAudioPlaybackService audioPlaybackService;

        private readonly ISimpleGlobalHotkeyService globalHotkeyService;

        public ApplicationHotkeyService(
            ISimpleGlobalHotkeyService globalHotkeyService,
            IAppSettingService appSettingService,
            IAudioRecordingService audioRecordingService,
            IAudioPlaybackService audioPlaybackService)
        {
            this.globalHotkeyService = globalHotkeyService;
            this.appSettingService = appSettingService;
            this.audioRecordingService = audioRecordingService;
            this.audioPlaybackService = audioPlaybackService;

            //ResetAndReadHotkeysFromConfig();
        }

        public void PauseCapturingHotkeys(bool pause = true)
        {
            globalHotkeyService.ProcessingHotkeys = !pause;
        }

        public void ResetAndReadHotkeysFromConfig(IActionService actionService)
        {
            globalHotkeyService.RemoveAllHotkeys();

            var currentDispatcher = Dispatcher.CurrentDispatcher;

            // hotkey action should not make hotkeyservice/hook wait
            globalHotkeyService.AddOrUpdateOnReleaseHotkey(
                appSettingService.HotKey_StartStopRecording,
                () => new Task(() => { if (audioPlaybackService.Stopped) { currentDispatcher.Invoke(() => actionService.ToggleStartStopRecording()); } }).Start());
        }

        public void ResumeCapturingHotkeys()
        {
            globalHotkeyService.ProcessingHotkeys = true;
        }
    }
}
