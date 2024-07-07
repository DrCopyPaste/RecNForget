using NAudio.Wave;
using RecNForget.Services.Contracts.Events;
using System;

namespace RecNForget.Services.Contracts
{
    public interface IAudioPlaybackService
    {
        string ReplayStartAudioFeedbackPath { get; }

        string ReplayStopAudioFeedbackPath { get; }

        string RecordStartAudioFeedbackPath { get; }

        string RecordStopAudioFeedbackPath { get; }

        int ItemsCount { get; }

        PlaybackState PlaybackState { get; }

        event EventHandler<AudioPlaybackServiceEventArgs> AudioPlaybackChanged;

        bool QueueFile(string filePath);

        bool Play();

        void Pause();

        void Stop();

        void KillAudio(bool reset = false);

        string GetFileLengthInSecondsFormatted(string filePath);
        bool QueueFiles(string[] filePaths);
    }
}
