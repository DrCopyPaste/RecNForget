using System;

namespace RecNForget.WPF.Services.Contracts
{
    public interface IActionService
    {
        System.Windows.Controls.Control OwnerControl { get; set; }

        // skip to previous file
        void SelectPreviousFile();

        // skip to next file
        void SelectNextFile();

        // toggle play/pause selected file
        void TogglePlayPauseSelectedFile();

        // stop playing selected file (and closing it)
        void StopPlayingSelectedFile();

        // toggle start recording/ stop recording (and saving as file)
        void ToggleStartStopRecording();

        bool QueueAudioPlayback(string fileName = null, string startIndicatorFileName = null, string endIndicatorFileName = null);

        void TogglePlayPauseAudio();
        void ShowSettingsMenu();
        void ToggleSelectedFileControlVisibility();
        void ToggleOutputPathControlVisibility();
        void ShowHelpWindow();
        void ShowAboutWindow();
        void Exit();
        void ShowNewToApplicationWindow();
        void ShowNewToVersionDialog(Version currentFileVersion, Version lastInstalledVersion);
        void ShowRandomApplicationTip();
        void ToggleRecordingTimerControlVisibility();
    }
}
