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
        private readonly IActionService actionService;
        private readonly ISimpleGlobalHotkeyService globalHotkeyService;

        public ApplicationHotkeyService(
            ISimpleGlobalHotkeyService globalHotkeyService,
            IAppSettingService appSettingService,
            IAudioRecordingService audioRecordingService,
            IAudioPlaybackService audioPlaybackService,
            IActionService actionService)
        {
            this.globalHotkeyService = globalHotkeyService;
            this.appSettingService = appSettingService;
            this.audioRecordingService = audioRecordingService;
            this.audioPlaybackService = audioPlaybackService;

            this.actionService = actionService;
            //ResetAndReadHotkeysFromConfig();
        }

        public void PauseCapturingHotkeys(bool pause = true)
        {
            globalHotkeyService.ProcessingHotkeys = !pause;
        }

        public void ResetAndReadHotkeysFromConfig()
        {
            globalHotkeyService.RemoveAllHotkeys();

            var currentDispatcher = Dispatcher.CurrentDispatcher;

            // hotkey action should not make hotkeyservice/hook wait
            globalHotkeyService.AddOrUpdateOnReleaseHotkey(
                PressedKeysInfo.FromString(appSettingService.HotKey_StartStopRecording),
                () => { if (audioPlaybackService.Stopped) { currentDispatcher.Invoke(() => actionService.ToggleStartStopRecording()); } });
            //() =>
            //{
            //    var task = Task.Run(() => { if (audioPlaybackService.Stopped) { currentDispatcher.Invoke(() => actionService.ToggleStartStopRecording()); } });
            //    task.Start();
            //    task.Wait();
            //    task.Dispose();
            //});
        }

        public void ResumeCapturingHotkeys()
        {
            globalHotkeyService.ProcessingHotkeys = true;
        }
    }
}
