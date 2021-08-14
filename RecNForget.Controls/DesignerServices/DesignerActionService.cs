using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;

namespace RecNForget.Services.Designer
{
    public class DesignerActionService : IActionService
    {
        public Control OwnerControl { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string CurrentRecordingStartAfterTimer { get => "0:00:13:37"; set => throw new NotImplementedException(); }
        public string CurrentRecordingStopAfterTimer { get => "0:00:31:41"; set => throw new NotImplementedException(); }
        public bool TimerForRecordingStartAfterNotRunning { get => false; set => throw new NotImplementedException(); }
        public bool TimerForRecordingStopAfterNotRunning { get => false; set => throw new NotImplementedException(); }

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

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void ExportSelectedFile()
        {
            throw new NotImplementedException();
        }

        public void OpenOutputFolderInExplorer()
        {
            throw new System.NotImplementedException();
        }

        public bool QueueAudioPlayback(string fileName = null, string startIndicatorFileName = null, string endIndicatorFileName = null)
        {
            throw new System.NotImplementedException();
        }

        public void ResetDispatcherTimer()
        {
            throw new NotImplementedException();
        }

        public void SelectNextFile()
        {
            throw new System.NotImplementedException();
        }

        public void SelectPreviousFile()
        {
            throw new System.NotImplementedException();
        }

        public void ShowAboutWindow()
        {
            throw new System.NotImplementedException();
        }

        public void ShowApplicationMenu()
        {
            throw new System.NotImplementedException();
        }

        public void ShowHelpWindow()
        {
            throw new System.NotImplementedException();
        }

        public void ShowNewToApplicationWindow()
        {
            throw new NotImplementedException();
        }

        public void ShowNewToVersionDialog(Version currentFileVersion, Version lastInstalledVersion)
        {
            throw new NotImplementedException();
        }

        public void ShowRandomApplicationTip()
        {
            throw new NotImplementedException();
        }

        public void ShowSettingsMenu()
        {
            throw new System.NotImplementedException();
        }

        public void StartTimerToStartRecordingAfter()
        {
            throw new NotImplementedException();
        }

        public void StartTimerToStopRecordingAfter()
        {
            throw new NotImplementedException();
        }

        public void StopPlayingSelectedFile()
        {
            throw new System.NotImplementedException();
        }

        public void ToggleOutputPathControlVisibility()
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

        public void ToggleRecordingTimerControlVisibility()
        {
            throw new NotImplementedException();
        }

        public void ToggleSelectedFileControlVisibility()
        {
            throw new System.NotImplementedException();
        }

        public void ToggleStartStopRecording()
        {
            throw new System.NotImplementedException();
        }
    }
}