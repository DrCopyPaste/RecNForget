using FMUtils.KeyboardHook;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Services
{
    public class AudioRecordingService
    {
        private Action startRecordingAction;
        private Action stopRecordingAction;

        // will be newly instantiated every time record starting is triggered
        private static WasapiLoopbackCapture captureInstance;

        public string CurrentFileName { get; set; } = string.Empty;
        public string LastFileName { get; set; } = string.Empty;

        public bool CurrentlyRecording { get; set; }


        public AudioRecordingService(Action startRecordingAction, Action stopRecordingAction)
        {
            this.startRecordingAction = startRecordingAction;
            this.stopRecordingAction = stopRecordingAction;

            CurrentlyRecording = false;
        }

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

            var settingOutputPath = AppSettingHelper.GetAppConfigSetting("OutputPath");
            var settingFilenamePrefix = AppSettingHelper.GetAppConfigSetting("FilenamePrefix");

            string outputFilePathPattern = @"{0}\{1}{2}.wav";

            FileInfo file;

            do
            {
                // datestrings > GUIDS :D
                file = new FileInfo(string.Format(outputFilePathPattern, settingOutputPath, settingFilenamePrefix, DateTime.Now.ToString("yyyyMMdd-HHmmssfff")));
            } while (file.Exists);

            DirectoryInfo directory = new DirectoryInfo(file.DirectoryName);
            if (!directory.Exists)
            {
                directory.Create();
            }

            captureInstance = new WasapiLoopbackCapture();

            // Redefine the audio writer instance with the given configuration
            WaveFileWriter RecordedAudioWriter = new WaveFileWriter(file.FullName, captureInstance.WaveFormat);
            CurrentFileName = file.FullName;

            // When the capturer receives audio, start writing the buffer into the mentioned file
            captureInstance.DataAvailable += (s, a) =>
            {
                // Write buffer into the file of the writer instance
                RecordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
            };

            // When the Capturer Stops, dispose instances of the capturer and writer
            captureInstance.RecordingStopped += (s, a) =>
            {
                RecordedAudioWriter.Dispose();
                RecordedAudioWriter = null;
                captureInstance.Dispose();
            };

            // Actions passed to HotkeyService as Parameter shall be executed after all start actions besides recording itself
            startRecordingAction();

            // Start audio recording !
            captureInstance.StartRecording();
        }

        public void StopRecording()
        {
            CurrentlyRecording = false;

            captureInstance.StopRecording();
            LastFileName = CurrentFileName;
            CurrentFileName = string.Empty;

            // Actions passed to HotkeyService as Parameter shall be executed after all stop actions
            stopRecordingAction();
        }
    }
}
