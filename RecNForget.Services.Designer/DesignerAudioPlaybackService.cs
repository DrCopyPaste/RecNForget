using System;
using System.ComponentModel;
using NAudio.Wave;
using RecNForget.Services.Contracts;

namespace RecNForget.Services.Designer
{
    public class DesignerAudioPlaybackService : IAudioPlaybackService
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Paused => false;

        public bool Playing => false;

        public bool PlayingOrPaused => false;

        public bool Stopped => true;

        public PlaybackState PlaybackState => PlaybackState.Stopped;

        public int ItemsCount => 42;

        public string ReplayStartAudioFeedbackPath => throw new NotImplementedException();

        public string ReplayStopAudioFeedbackPath => throw new NotImplementedException();

        public string RecordStartAudioFeedbackPath => throw new NotImplementedException();

        public string RecordStopAudioFeedbackPath => throw new NotImplementedException();

        public bool QueueFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool Play()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void KillAudio(bool reset = false)
        {
            throw new NotImplementedException();
        }
    }
}