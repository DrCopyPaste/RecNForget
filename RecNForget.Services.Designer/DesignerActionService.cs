using System.ComponentModel;
using System.Windows;
using RecNForget.Services.Contracts;

namespace RecNForget.Services.Designer
{
    public class DesignerActionService : IActionService
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void ChangeFileNamePattern()
        {
            throw new System.NotImplementedException();
        }

        public void ChangeOutputFolder()
        {
            throw new System.NotImplementedException();
        }

        public void ChangeSelectedFileName()
        {
            throw new System.NotImplementedException();
        }

        public void CheckForUpdates(bool showMessages = false)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteSelectedFile()
        {
            throw new System.NotImplementedException();
        }

        public void OpenOutputFolderInExplorer()
        {
            throw new System.NotImplementedException();
        }

        public bool QueueAudioPlayback(string fileName = null, string startIndicatorFileName = null, string endIndicatorFileName = null)
        {
            throw new System.NotImplementedException();
        }

        public void SelectNextFile()
        {
            throw new System.NotImplementedException();
        }

        public void SelectPreviousFile()
        {
            throw new System.NotImplementedException();
        }

        public void ShowApplicationMenu()
        {
            throw new System.NotImplementedException();
        }

        public void StopPlayingSelectedFile()
        {
            throw new System.NotImplementedException();
        }

        public void TogglePlayPauseAudio()
        {
            throw new System.NotImplementedException();
        }

        public void TogglePlayPauseSelectedFile()
        {
            throw new System.NotImplementedException();
        }

        public void ToggleStartStopRecording()
        {
            throw new System.NotImplementedException();
        }
    }
}