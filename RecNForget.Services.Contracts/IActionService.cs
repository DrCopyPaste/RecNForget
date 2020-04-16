using System.ComponentModel;
using System.Windows;

namespace RecNForget.Services.Contracts
{
    public interface IActionService : INotifyPropertyChanged
    {
        // change output directory (with dialog)
        void ChangeOutputFolder();

        // change file name pattern (with dialog)
        void ChangeFileNamePattern();

        // open explorer for output folder (select file if there is any)
        void OpenOutputFolderInExplorer();

        // skip to previous file
        void SelectPreviousFile();

        // skip to next file
        void SelectNextFile();

        // toggle play/pause selected file
        void TogglePlayPauseSelectedFile();

        // stop playing selected file (and closing it)
        void StopPlayingSelectedFile();

        // toggle start recording/ stop recording (and saving as file)
        void ToggleStartStopRecording();

        // change selected file name (with dialog)
        void ChangeSelectedFileName(DependencyObject ownerControl);

        // delete selected file (with dialog)
        void DeleteSelectedFile(DependencyObject ownerControl);
    }
}
