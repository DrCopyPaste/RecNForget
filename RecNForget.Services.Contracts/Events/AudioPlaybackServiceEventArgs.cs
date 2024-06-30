using NAudio.Wave;

namespace RecNForget.Services.Contracts.Events;

public class AudioPlaybackServiceEventArgs
{
    public PlaybackState PlaybackState { get; private set; }

    public AudioPlaybackServiceEventArgs(PlaybackState playbackState)
    {
        PlaybackState = playbackState;
    }
}
