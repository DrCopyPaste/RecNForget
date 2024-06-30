using RecNForget.Services.Contracts.Events;
using System;
using System.ComponentModel;
using System.Windows;

namespace RecNForget.Services.Contracts
{
    public interface IAudioRecordingService
    {
        string CurrentRecordingStartAfterTimer { get; set; }
        string CurrentRecordingStopAfterTimer { get; set; }
        bool TimerForRecordingStartAfterNotRunning { get; set; }
        bool TimerForRecordingStopAfterNotRunning { get; set; }
        string CurrentFileName { get; }

        string LastFileName { get; }

        bool CurrentlyRecording { get; }

        bool CurrentlyNotRecording { get; }

        event EventHandler<AudioRecordingServiceEventArgs> AudioRecordingStateChanged;

        // starts or stops recording according to CurrentlyRecording state
        void ToggleRecording();

        void StartRecording();

        void StopRecording();

        string GetTargetPathTemplateString();
        void StartTimerToStartRecordingAfter();
        void StartTimerToStopRecordingAfter();
        void ResetStartAfterDispatcherTimer();
        void ResetStopAfterDispatcherTimer();
        void ResetAllTimers();
    }
}
