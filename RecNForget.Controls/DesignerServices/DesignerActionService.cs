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

        public void ChangeTheme(string themeName)
        {
            throw new NotImplementedException();
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