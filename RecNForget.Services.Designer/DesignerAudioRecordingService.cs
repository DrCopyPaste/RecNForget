using System;
using System.ComponentModel;
using RecNForget.Services.Contracts;

namespace RecNForget.Services.Designer
{
    public class DesignerAudioRecordingService : IAudioRecordingService
    {
        public string CurrentFileName => throw new NotImplementedException();

        public string LastFileName => throw new NotImplementedException();

        public bool CurrentlyRecording => throw new NotImplementedException();

        public bool CurrentlyNotRecording => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;

        public string GetTargetPathTemplateString()
        {
            throw new NotImplementedException();
        }

        public void StartRecording()
        {
            throw new NotImplementedException();
        }

        public void StopRecording()
        {
            throw new NotImplementedException();
        }

        public void ToggleRecording()
        {
            throw new NotImplementedException();
        }
    }
}