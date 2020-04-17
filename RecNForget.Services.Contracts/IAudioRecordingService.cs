using System.ComponentModel;
using System.Windows;

namespace RecNForget.Services.Contracts
{
    public interface IAudioRecordingService : INotifyPropertyChanged
    {
        string CurrentFileName { get; }

        string LastFileName { get; }

        bool CurrentlyRecording { get; }

        bool CurrentlyNotRecording { get; }

        // starts or stops recording according to CurrentlyRecording state
        void ToggleRecording();

        void StartRecording();

        void StopRecording();

        string GetTargetPathTemplateString();
    }
}
