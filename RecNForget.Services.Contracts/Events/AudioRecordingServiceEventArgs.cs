namespace RecNForget.Services.Contracts.Events;

public class AudioRecordingServiceEventArgs
{
    public bool IsRecording { get; private set; }

    public AudioRecordingServiceEventArgs(bool isRecording)
    {
        IsRecording = isRecording;
    }
}
