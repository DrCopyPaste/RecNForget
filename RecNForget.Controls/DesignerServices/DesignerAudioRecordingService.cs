using System;
using System.ComponentModel;
using RecNForget.Services.Contracts;

namespace RecNForget.Services.Designer
{
    public class DesignerAudioRecordingService : IAudioRecordingService
    {
        public string CurrentFileName => "thisDemo.wav";

        public string LastFileName => "lastDemo.wav";

        public bool CurrentlyRecording => false;

        public bool CurrentlyNotRecording => true;

        public string CurrentRecordingStartAfterTimer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string CurrentRecordingStopAfterTimer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TimerForRecordingStartAfterNotRunning { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TimerForRecordingStopAfterNotRunning { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event PropertyChangedEventHandler PropertyChanged;

        public string GetTargetPathTemplateString() => @"C:\imaginary\Path\demo(guid).wav";

        public void ResetAllTimers()
        {
            throw new NotImplementedException();
        }

        public void ResetStartAfterDispatcherTimer()
        {
            throw new NotImplementedException();
        }

        public void ResetStopAfterDispatcherTimer()
        {
            throw new NotImplementedException();
        }

        public void StartRecording() => throw new NotImplementedException();

        public void StartTimerToStartRecordingAfter()
        {
            throw new NotImplementedException();
        }

        public void StartTimerToStopRecordingAfter()
        {
            throw new NotImplementedException();
        }

        public void StopRecording() => throw new NotImplementedException();

        public void ToggleRecording() => throw new NotImplementedException();
    }
}