using System.ComponentModel;
using System.IO;
using NAudio.Wave;

namespace RecNForget.Services.Contracts
{
    public interface IAudioPlaybackService : INotifyPropertyChanged
    {
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
    }
}
