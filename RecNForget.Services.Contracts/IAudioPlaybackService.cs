using System.ComponentModel;
using System.IO;
using NAudio.Wave;

namespace RecNForget.Services.Contracts
{
    public interface IAudioPlaybackService : INotifyPropertyChanged
    {
        string ReplayStartAudioFeedbackPath { get; }

        string ReplayStopAudioFeedbackPath { get; }

        string RecordStartAudioFeedbackPath { get; }

        string RecordStopAudioFeedbackPath { get; }

        int ItemsCount { get; }

        bool Paused { get; }

        bool Playing { get; }

        bool PlayingOrPaused { get; }

        bool Stopped { get; }

        PlaybackState PlaybackState { get; }

        bool QueueFile(string filePath);

        bool Play();

        void Pause();

        void Stop();

        void KillAudio(bool reset = false);
        bool QueueAudioPlayback(string fileName = null, string startIndicatorFileName = null, string endIndicatorFileName = null);
        void TogglePlayPauseAudio();
    }
}
