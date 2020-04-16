using System;
using System.ComponentModel;
using System.IO;
using RecNForget.Services.Contracts;

namespace RecNForget.Services.Designer
{
    public class DesignerSelectedFileService : ISelectedFileService
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public FileInfo SelectedFile => new FileInfo(@"C:\imaginaryPath\2020-04-12-190654512_A_file.wav");

        public bool HasSelectedFile => true;

        public string SelectedFileDisplay
        {
            get => @"C:\imaginaryPath\2020-04-12-190654512_A_file.wav";
            set => throw new NotImplementedException();
        }

        public bool SelectFile(FileInfo file)
        {
            throw new NotImplementedException();
        }

        public bool SelectLatestFile()
        {
            throw new NotImplementedException();
        }

        public bool SelectNextFile()
        {
            throw new NotImplementedException();
        }

        public bool SelectPrevFile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// tries to delete the current one and select previous file
        /// </summary>
        /// <returns></returns>
        public bool DeleteSelectedFile()
        {
            throw new NotImplementedException();
        }

        public bool RenameSelectedFileWithoutExtension(string newNameWithoutExtension)
        {
            throw new NotImplementedException();
        }
    }
}
