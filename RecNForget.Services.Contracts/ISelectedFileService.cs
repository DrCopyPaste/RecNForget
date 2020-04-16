using System;
using System.ComponentModel;
using System.IO;

namespace RecNForget.Services.Contracts
{
    public interface ISelectedFileService : INotifyPropertyChanged
    {
        FileInfo SelectedFile { get; }

        bool HasSelectedFile { get; }

        string SelectedFileDisplay { get; set; }

        bool SelectFile(FileInfo file);

        bool SelectLatestFile();

        bool SelectNextFile();

        bool SelectPrevFile();

        bool DeleteSelectedFile();

        bool RenameSelectedFileWithoutExtension(string newNameWithoutExtension);
    }
}
