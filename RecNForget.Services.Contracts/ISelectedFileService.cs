using RecNForget.Services.Contracts.Events;
using System;
using System.IO;

namespace RecNForget.Services.Contracts;

public interface ISelectedFileService
{
    event EventHandler<SelectedFileServiceEventArgs> SelectedFileChanged;
    FileInfo SelectedFile { get; }
    bool HasSelectedFile { get; }

    bool SelectFile(FileInfo file);

    bool SelectLatestFile();

    bool SelectNextFile();

    bool SelectPrevFile();

    bool DeleteSelectedFile();

    bool RenameSelectedFileWithoutExtension(string newNameWithoutExtension);
    string ExportFile(string preferredFileName = "");
}
