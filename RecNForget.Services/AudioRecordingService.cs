using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using NAudio.Wave;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;

namespace RecNForget.Services
{
    public class AudioRecordingService : IAudioRecordingService
    {
        private static string outputFilePathPattern = @"{0}\{1}.wav";
        private static string outputFileDateFormat = "yyyy_MM_dd_HH_mm_ss_fff";

        // will be newly instantiated every time record starting is triggered
        private static WasapiLoopbackCapture captureInstance;

        private readonly IAppSettingService appSettingService;
        private readonly IAudioPlaybackService audioPlaybackService;
        private readonly ISelectedFileService selectedFileService;

        private DispatcherTimer startAfterDispatcherTimer = new DispatcherTimer();
        private DispatcherTimer stopAfterdispatcherTimer = new DispatcherTimer();

        private string GetCurrentStartAfterDispatcherTimeString()
        {
            return startAfterDispatcherTimerCurrentTime.ToString(Formats.TimeSpanFormat);
        }

        private string GetCurrentStopAfterDispatcherTimeString()
        {
            return stopAfterDispatcherTimerCurrentTime.ToString(Formats.TimeSpanFormat);
        }

        private TimeSpan startAfterDispatcherTimerCurrentTime = TimeSpan.Zero;
        private TimeSpan stopAfterDispatcherTimerCurrentTime = TimeSpan.Zero;

        public AudioRecordingService(IAppSettingService appSettingService, IAudioPlaybackService audioPlaybackService, ISelectedFileService selectedFileService)
        {
            this.appSettingService = appSettingService;
            this.audioPlaybackService = audioPlaybackService;
            this.selectedFileService = selectedFileService;

            CurrentlyRecording = false;
            CurrentlyNotRecording = true;
            UpdateProperties();

            startAfterDispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            startAfterDispatcherTimer.Tick += StartAfter_DispatcherTimer_Tick;
            stopAfterdispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            stopAfterdispatcherTimer.Tick += StopAfter_DispatcherTimer_Tick;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string CurrentFileName { get; private set; } = string.Empty;

        public string LastFileName { get; private set; } = string.Empty;

        public bool CurrentlyRecording { get; private set; } = false;

        public bool CurrentlyNotRecording { get; private set; } = true;

        // starts or stops recording according to CurrentlyRecording state
        private void ToggleRecordingInternal()
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

        private bool timerForRecordingStartAfterNotRunning = true;
        public bool TimerForRecordingStartAfterNotRunning
        {
            get { return timerForRecordingStartAfterNotRunning; }
            set { timerForRecordingStartAfterNotRunning = value; OnPropertyChanged(); }
        }

        private bool timerForRecordingStopAfterNotRunning = true;

        public bool TimerForRecordingStopAfterNotRunning
        {
            get { return timerForRecordingStopAfterNotRunning; }
            set { timerForRecordingStopAfterNotRunning = value; OnPropertyChanged(); }
        }


        private string currentRecordingStopAfterTimer = "0:00:00:00";

        public string CurrentRecordingStopAfterTimer
        {
            get { return currentRecordingStopAfterTimer; }
            set { currentRecordingStopAfterTimer = value; OnPropertyChanged(); }
        }

        private string currentRecordingStartAfterTimer = "0:00:00:00";

        public string CurrentRecordingStartAfterTimer
        {
            get { return currentRecordingStartAfterTimer; }
            set { currentRecordingStartAfterTimer = value; OnPropertyChanged(); }
        }

        private void StartAfter_DispatcherTimer_Tick(object sender, EventArgs e)
        {
            startAfterDispatcherTimerCurrentTime = startAfterDispatcherTimerCurrentTime.Subtract(TimeSpan.FromSeconds(1));
            CurrentRecordingStartAfterTimer = GetCurrentStartAfterDispatcherTimeString();

            if (startAfterDispatcherTimerCurrentTime.TotalSeconds < 1)
            {
                if (!CurrentlyRecording)
                {
                    ToggleRecordingInternal();

                    if (appSettingService.RecordingTimerStopAfterIsEnabled)
                    {
                        StartTimerToStopRecordingAfter();
                        return;
                    }
                }

                ResetStartAfterDispatcherTimer();
            }
        }

        private void StopAfter_DispatcherTimer_Tick(object sender, EventArgs e)
        {
            stopAfterDispatcherTimerCurrentTime = stopAfterDispatcherTimerCurrentTime.Subtract(TimeSpan.FromSeconds(1));
            CurrentRecordingStopAfterTimer = GetCurrentStopAfterDispatcherTimeString();

            if (stopAfterDispatcherTimerCurrentTime.TotalSeconds < 1)
            {
                // ToDo more than two timer options?
                // timer finished, trigger respective method either way
                if (!TimerForRecordingStartAfterNotRunning)
                {
                    if (!CurrentlyRecording)
                    {
                        ToggleRecordingInternal();

                        if (appSettingService.RecordingTimerStopAfterIsEnabled)
                        {
                            StartTimerToStopRecordingAfter();
                            return;
                        }
                    }
                }
                else
                {
                    if (CurrentlyRecording)
                    {
                        ToggleRecordingInternal();
                    }
                }

                ResetStopAfterDispatcherTimer();
            }
        }

        public void StartTimerToStartRecordingAfter()
        {
            // reset all other possibly running timers
            ResetStartAfterDispatcherTimer();
            startAfterDispatcherTimerCurrentTime = TimeSpan.ParseExact(appSettingService.RecordingTimerStartAfterMax, Formats.TimeSpanFormat, CultureInfo.InvariantCulture);
            CurrentRecordingStartAfterTimer = GetCurrentStartAfterDispatcherTimeString();
            TimerForRecordingStartAfterNotRunning = false;

            startAfterDispatcherTimer.Start();
        }

        public void StartTimerToStopRecordingAfter()
        {
            // reset all other possibly running timers
            ResetStopAfterDispatcherTimer();
            stopAfterDispatcherTimerCurrentTime = TimeSpan.ParseExact(appSettingService.RecordingTimerStopAfterMax, Formats.TimeSpanFormat, CultureInfo.InvariantCulture);
            CurrentRecordingStopAfterTimer = GetCurrentStopAfterDispatcherTimeString();
            TimerForRecordingStopAfterNotRunning = false;

            stopAfterdispatcherTimer.Start();
        }

        public void ResetStartAfterDispatcherTimer()
        {
            startAfterDispatcherTimer.Stop();
            TimerForRecordingStartAfterNotRunning = true;
            startAfterDispatcherTimerCurrentTime = TimeSpan.ParseExact(appSettingService.RecordingTimerStartAfterMax, Formats.TimeSpanFormat, CultureInfo.InvariantCulture);
            CurrentRecordingStartAfterTimer = appSettingService.RecordingTimerStartAfterMax;
        }

        public void ResetStopAfterDispatcherTimer()
        {
            stopAfterdispatcherTimer.Stop();
            TimerForRecordingStopAfterNotRunning = true;
            stopAfterDispatcherTimerCurrentTime = TimeSpan.ParseExact(appSettingService.RecordingTimerStopAfterMax, Formats.TimeSpanFormat, CultureInfo.InvariantCulture);
            CurrentRecordingStopAfterTimer = appSettingService.RecordingTimerStopAfterMax;
        }

        public void ResetAllTimers()
        {
            ResetStartAfterDispatcherTimer();
            ResetStopAfterDispatcherTimer();
        }

        public void ToggleRecording()
        {
            // if no timers are configured, reset all timers and toggle record directly
            if (!appSettingService.RecordingTimerStartAfterIsEnabled && !appSettingService.RecordingTimerStopAfterIsEnabled)
            {
                ResetAllTimers();
                ToggleRecordingInternal();
                return;
            }

            // if currently recording, all timers can be ignored - stop after recording timer is ignored
            if (CurrentlyRecording)
            {
                ResetAllTimers();
                ToggleRecordingInternal();
                return;
            }

            // if NOT currently recording, recording MIGHT be delayed by a timespan set in 
            if (appSettingService.RecordingTimerStartAfterIsEnabled)
            {
                // override start to recording timer if action was triggered again
                if (startAfterDispatcherTimer.IsEnabled)
                {
                    ResetAllTimers();
                    ToggleRecordingInternal();

                    if (appSettingService.RecordingTimerStopAfterIsEnabled)
                    {
                        StartTimerToStopRecordingAfter();
                    }

                    return;
                }

                StartTimerToStartRecordingAfter();
            }
            else
            {
                ResetAllTimers();
                ToggleRecordingInternal();

                if (appSettingService.RecordingTimerStopAfterIsEnabled)
                {
                    StartTimerToStopRecordingAfter();
                }
            }
        }

        public void StartRecording()
        {
            CurrentlyRecording = true;
            CurrentlyNotRecording = false;
            UpdateProperties();

            // play pre recording signal
            if (appSettingService.PlayAudioFeedBackMarkingStartAndStopRecording)
            {
                audioPlaybackService.KillAudio(reset: true);

                audioPlaybackService.QueueFile(audioPlaybackService.RecordStartAudioFeedbackPath);
                audioPlaybackService.Play();

                while (audioPlaybackService.PlaybackState != PlaybackState.Stopped) { }

                audioPlaybackService.KillAudio(reset: true);
            }


            var file = GetUniqueWorkingFileName(appSettingService.OutputPath, "wav");
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

                var targetFile = GetUniqueTargetFileName(appSettingService.OutputPath, "wav");
                File.Move(CurrentFileName, targetFile.FullName);

                LastFileName = targetFile.FullName;
                CurrentFileName = string.Empty;
                UpdateProperties();

                // play post recording signal
                if (appSettingService.PlayAudioFeedBackMarkingStartAndStopRecording || appSettingService.AutoReplayAudioAfterRecording)
                {
                    if (appSettingService.PlayAudioFeedBackMarkingStartAndStopRecording)
                    {
                        audioPlaybackService.QueueAudioPlayback(fileName: audioPlaybackService.RecordStopAudioFeedbackPath);
                    }

                    if (appSettingService.AutoReplayAudioAfterRecording)
                    {
                        audioPlaybackService.QueueAudioPlayback(
                            fileName: LastFileName,
                            startIndicatorFileName: appSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? audioPlaybackService.ReplayStartAudioFeedbackPath : null,
                            endIndicatorFileName: appSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying ? audioPlaybackService.ReplayStopAudioFeedbackPath : null);
                    }

                    audioPlaybackService.TogglePlayPauseAudio();
                }

                if (appSettingService.AutoSelectLastRecording)
                {
                    selectedFileService.SelectFile(new FileInfo(LastFileName));
                }
            };

            // Start audio recording !
            captureInstance.StartRecording();
        }

        private FileInfo GetUniqueWorkingFileName(string outputFolderPath = "", string extensionWithoutDot = "wav")
        {
            var outputFolder = new DirectoryInfo(outputFolderPath);
            if (!outputFolder.Exists)
            {
                outputFolder.Create();
            }

            FileInfo file;
            do
            {
                file = new FileInfo(Path.Combine(outputFolder.FullName, $"{Guid.NewGuid():N}.{extensionWithoutDot}"));
            } while (file.Exists);

            return file;
        }

        private FileInfo GetUniqueTargetFileName(string outputFolderPath = "", string extensionWithoutDot = "wav")
        {
            var outputFolder = new DirectoryInfo(outputFolderPath);
            if (!outputFolder.Exists)
            {
                outputFolder.Create();
            }

            FileInfo file;
            do
            {
                file = new FileInfo(GetFileNameWithReplacePlaceholders());
            } while (file.Exists);

            return file;
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
