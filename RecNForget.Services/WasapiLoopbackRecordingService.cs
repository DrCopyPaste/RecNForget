using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Services
{
    public class WasapiLoopbackRecordingService
    {
        // will be newly instantiated every time record starting is triggered
        private static WasapiLoopbackCapture captureInstance;

        private string workingFileName = string.Empty;

        public Action<string> OnRecordingStop;

        public WasapiLoopbackRecordingService()
        {

        }

        public void RecordTo(string workingOutputFileName, Func<string> outputFileGenerator)
        {
            var file = new FileInfo(workingOutputFileName);
            captureInstance = new WasapiLoopbackCapture();

            // Redefine the audio writer instance with the given configuration
            WaveFileWriter recordedAudioWriter = new WaveFileWriter(file.FullName, captureInstance.WaveFormat);
            workingFileName = file.FullName;

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

                var targetFileName = outputFileGenerator();
                File.Move(workingFileName, targetFileName);

                OnRecordingStop?.Invoke(targetFileName);
            };

            // Start audio recording !
            captureInstance.StartRecording();
        }

        public void StopRecording()
        {
            captureInstance.StopRecording();
        }
    }
}
