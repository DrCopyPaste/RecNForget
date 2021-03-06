﻿using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using NAudio.Wave;
using RecNForget.Services.Contracts;

namespace RecNForget.Services
{
    public class AudioRecordingService : IAudioRecordingService
    {
        private static string outputFilePathPattern = @"{0}\{1}.wav";
        private static string outputFileDateFormat = "yyyy_MM_dd_HH_mm_ss_fff";

        // will be newly instantiated every time record starting is triggered
        private static WasapiLoopbackCapture captureInstance;

        private readonly IAppSettingService appSettingService;

        public AudioRecordingService(IAppSettingService appSettingService)
        {
            this.appSettingService = appSettingService;

            CurrentlyRecording = false;
            CurrentlyNotRecording = true;
            UpdateProperties();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string CurrentFileName { get; private set; } = string.Empty;

        public string LastFileName { get; private set; } = string.Empty;

        public bool CurrentlyRecording { get; private set; } = false;

        public bool CurrentlyNotRecording { get; private set; } = true;

        // starts or stops recording according to CurrentlyRecording state
        public void ToggleRecording()
        {
            if (!CurrentlyRecording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }

        public void StartRecording()
        {
            CurrentlyRecording = true;
            CurrentlyNotRecording = false;
            UpdateProperties();

            FileInfo file;
            do
            {
                file = new FileInfo(GetFileNameWithReplacePlaceholders());
            }
            while (file.Exists);

            DirectoryInfo directory = new DirectoryInfo(file.DirectoryName);
            if (!directory.Exists)
            {
                directory.Create();
            }

            captureInstance = new WasapiLoopbackCapture();

            // Redefine the audio writer instance with the given configuration
            WaveFileWriter recordedAudioWriter = new WaveFileWriter(file.FullName, captureInstance.WaveFormat);
            CurrentFileName = file.FullName;

            // When the capturer receives audio, start writing the buffer into the mentioned file
            captureInstance.DataAvailable += (s, a) =>
            {
                // Write buffer into the file of the writer instance
                recordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
            };

            // When the Capturer Stops, dispose instances of the capturer and writer
            captureInstance.RecordingStopped += (s, a) =>
            {
                recordedAudioWriter.Dispose();
                recordedAudioWriter = null;
                captureInstance.Dispose();

                LastFileName = CurrentFileName;
                CurrentFileName = string.Empty;
                UpdateProperties();
            };

            // Start audio recording !
            captureInstance.StartRecording();
        }

        public void StopRecording()
        {
            CurrentlyRecording = false;
            CurrentlyNotRecording = true;
            captureInstance.StopRecording();
        }

        public string GetTargetPathTemplateString()
        {
            return string.Format(AudioRecordingService.outputFilePathPattern, appSettingService.OutputPath, appSettingService.FilenamePrefix);
        }

        private string GetFileNameWithReplacePlaceholders()
        {
            bool nameUniquePlaceholderFound = false;
            string tempString = string.Format(AudioRecordingService.outputFilePathPattern, appSettingService.OutputPath, appSettingService.FilenamePrefix);

            if (tempString.Contains("(Date)"))
            {
                nameUniquePlaceholderFound = true;
                tempString = tempString.Replace("(Date)", DateTime.Now.ToString(AudioRecordingService.outputFileDateFormat));
            }
            else if (tempString.Contains("(Guid)"))
            {
                nameUniquePlaceholderFound = true;
                tempString = tempString.Replace("(Guid)", Guid.NewGuid().ToString());
            }

            // if there is no placeholder in prefix that makes the file name somewhat unique, we just append a BEAUTIFUL timestamp
            // this is not an else, because there will be other placeholders that are NOT intended to make file names unique but, just add further information
            // for suppporting (Number) - sequential number placeholder
            // we first need to find out if there already exist files with the desired pattern and look for the highest number (or the first free slot, this might be messy if users intentionally create future sequential number files)
            if (!nameUniquePlaceholderFound)
            {
                var fileInfo = new FileInfo(tempString);
                string directory = fileInfo.DirectoryName;
                string filename = Path.GetFileNameWithoutExtension(fileInfo.Name) + "_" + DateTime.Now.ToString(AudioRecordingService.outputFileDateFormat);
                string extension = fileInfo.Extension;

                tempString = Path.Combine(directory, filename + extension);
            }

            return tempString;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateProperties()
        {
            OnPropertyChanged(nameof(CurrentlyNotRecording));
            OnPropertyChanged(nameof(CurrentlyRecording));
        }
    }
}
