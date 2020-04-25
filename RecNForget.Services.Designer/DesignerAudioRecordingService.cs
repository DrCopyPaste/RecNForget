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

        public event PropertyChangedEventHandler PropertyChanged;

        public string GetTargetPathTemplateString() => @"C:\imaginary\Path\demo(guid).wav";

        public void StartRecording() => throw new NotImplementedException();

        public void StopRecording() => throw new NotImplementedException();

        public void ToggleRecording() => throw new NotImplementedException();
    }
}