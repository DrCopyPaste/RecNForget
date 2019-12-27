using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Services
{
	// all SelectFile methods return false if that file does not exist (or if there is no file selected)

	// use SelectFile method to select a known file
	// use SelectLatestFile method to select the newest file
	// use SelectNextFile to move from SelectedFile to the next one
	// use SelectPrevFile to move from SelectedFile to the prev one
	public class SelectedFileService : INotifyPropertyChanged
	{
		private bool hasSelectedFile;
		private string selectedFileDisplay;

		public SelectedFileService()
		{
			HasSelectedFile = false;
			SelectedFileDisplay = "(no file found or selected)";
		}

		public FileInfo SelectedFile { get; set; }

		public bool HasSelectedFile
		{
			get
			{
				return hasSelectedFile;
			}
			set
			{
				hasSelectedFile = value;
				OnPropertyChanged();
			}
		}

		public string SelectedFileDisplay
		{
			get
			{
				return selectedFileDisplay;
			}
			set
			{
				selectedFileDisplay = value;
				OnPropertyChanged();
			}
		}

		public bool SelectFile(FileInfo file)
		{
			if (file == null || !file.Exists)
			{
				SelectedFile = null;
				HasSelectedFile = false;
				SelectedFileDisplay = "(no file found or selected)";
				return false;
			}
			else
			{
				SelectedFile = file;
				HasSelectedFile = true;
				SelectedFileDisplay = file.Name;
				return true;
			}
		}

		public bool SelectLatestFile()
		{
			var allFiles = GetOutputFiles();

			if (allFiles.Any())
			{
				return SelectFile(allFiles.First());
			}
			else
			{
				return false;
			}
		}

		public bool SelectNextFile()
		{
			var allFiles = GetOutputFiles();

			if (SelectedFile != null && SelectedFile.Exists && allFiles.Any())
			{
				var tempInfo = allFiles.First(f => f.FullName == SelectedFile.FullName);

				int currentIndex = allFiles.IndexOf(tempInfo);
				currentIndex++;

				if (currentIndex >= allFiles.Count)
				{
					currentIndex = 0;
				}

				return SelectFile(allFiles[currentIndex]);
			}
			else
			{
				return false;
			}
		}

		public bool SelectPrevFile()
		{
			var allFiles = GetOutputFiles();

			if (SelectedFile != null && SelectedFile.Exists && allFiles.Any())
			{
				var tempInfo = allFiles.First(f => f.FullName == SelectedFile.FullName);

				int currentIndex = allFiles.IndexOf(tempInfo);
				currentIndex--;

				if (currentIndex < 0)
				{
					currentIndex = allFiles.Count - 1;
				}

				return SelectFile(allFiles[currentIndex]);
			}
			else
			{
				return false;
			}
		}

		private string OutputPath
		{
			get
			{
				return AppSettingHelper.GetAppConfigSetting(AppSettingHelper.OutputPath);
			}
		}

		private List<FileInfo> GetOutputFiles()
		{
			DirectoryInfo directory = new DirectoryInfo(OutputPath);
			return directory.Exists ?
				directory.GetFiles("*.wav").OrderByDescending(x => x.CreationTime).ToList() :
				new List<FileInfo>();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
