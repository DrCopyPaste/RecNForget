using RecNForget.Services.Contracts.Events;
using System;
using System.ComponentModel;
using System.Windows;

namespace RecNForget.Services.Contracts
{
    public interface IAudioRecordingService
    {
        bool TimerForRecordingStartAfterNotRunning { get; }
        bool TimerForRecordingStopAfterNotRunning { get; }
        string CurrentFileName { get; }

        string LastFileName { get; }

        bool CurrentlyRecording { get; }

        bool CurrentlyNotRecording { get; }

        event EventHandler<AudioRecordingServiceEventArgs> AudioRecordingStateChanged;
        event EventHandler<TimerTickEventArgs> StartAfterTimerTick;
        event EventHandler<TimerTickEventArgs> StopAfterTimerTick;
        event EventHandler<TimerStateToggleEventArgs> StopAfterTimerStateToggle;
        event EventHandler<TimerStateToggleEventArgs> StartAfterTimerStateToggle;

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
