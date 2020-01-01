﻿using System;
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

		private Action noSelectableFileFoundAction;

		public SelectedFileService(Action noSelectableFileFoundAction = null)
		{
			this.noSelectableFileFoundAction = noSelectableFileFoundAction;

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

				if (!value)
				{
					noSelectableFileFoundAction?.Invoke();
				}

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

			if (allFiles.Any())
			{
				int currentIndex = 0;
				FileInfo tempInfo = allFiles.Any(f => f.FullName == SelectedFile.FullName) ? allFiles.First(f => f.FullName == SelectedFile.FullName) : null;

				if (tempInfo != null)
				{
					currentIndex = allFiles.IndexOf(tempInfo);
					currentIndex++;

					if (currentIndex >= allFiles.Count)
					{
						currentIndex = 0;
					}
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

			if (allFiles.Any())
			{
				int currentIndex = 0;
				FileInfo tempInfo = allFiles.Any(f => f.FullName == SelectedFile.FullName) ? allFiles.First(f => f.FullName == SelectedFile.FullName) : null;

				if (tempInfo != null)
				{
					currentIndex = allFiles.IndexOf(tempInfo);
					currentIndex--;

					if (currentIndex < 0)
					{
						currentIndex = allFiles.Count - 1;
					}
				}

				return SelectFile(allFiles[currentIndex]);
			}
			else
			{
				return false;
			}
		}

		public bool DeleteSelectedFile()
		{
			if (SelectedFile == null || !SelectedFile.Exists)
			{
				return false;
			}

			try
			{
				File.Delete(SelectedFile.FullName);
				SelectPrevFile();

				return true;
			}
			catch (Exception e)
			{
			}

			return false;
		}

		public bool RenameSelectedFileWithoutExtension(string newNameWithoutExtension)
		{
			if (SelectedFile == null || !SelectedFile.Exists)
			{
				return false;
			}

			try
			{
				string path = SelectedFile.DirectoryName;
				string extension = Path.GetExtension(SelectedFile.Name);
				string newFullName = Path.Combine(path, string.Concat(newNameWithoutExtension, extension));

				File.Move(SelectedFile.FullName, newFullName);
				SelectFile(new FileInfo(newFullName));

				return true;
			}
			catch (Exception e)
			{
			}

			return false;
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
