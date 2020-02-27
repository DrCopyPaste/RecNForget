using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using NAudio.Wave;

namespace RecNForget.Services
{
    public class AudioPlayListService : INotifyPropertyChanged
    {
        private readonly Action beforePlayAction = null;
        private readonly Action afterPauseAction = null;
        private readonly Action afterResumeAction = null;
        private readonly Action afterStopAction = null;
        private readonly Action queueErrorAction = null;
        private readonly List<string> filePathList = null;

        private WaveOutEvent audioOutputDevice = null;
        private AudioFileReader audioFileReader = null;
        private int currentPlayIndex = 0;

        public AudioPlayListService(Action beforePlayAction = null, Action afterStopAction = null, Action afterPauseAction = null, Action afterResumeAction = null, Action queueErrorAction = null)
        {
            this.beforePlayAction = beforePlayAction;
            this.afterStopAction = afterStopAction;
            this.afterPauseAction = afterPauseAction;
            this.afterResumeAction = afterResumeAction;
            this.queueErrorAction = queueErrorAction;

            filePathList = new List<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Paused
        {
            get
            {
                return PlaybackState == PlaybackState.Paused;
            }
        }

        public bool Playing
        {
            get
            {
                return PlaybackState == PlaybackState.Playing;
            }
        }

        public bool PlayingOrPaused
        {
            get
            {
                return Paused || Playing;
            }
        }

        public bool Stopped
        {
            get
            {
                return PlaybackState == PlaybackState.Stopped;
            }
        }

        public PlaybackState PlaybackState
        {
            get
            {
                return audioOutputDevice == null ? PlaybackState.Stopped : audioOutputDevice.PlaybackState;
            }
        }

        public bool QueueFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                filePathList.Add(filePath);

                return true;
            }

            queueErrorAction?.Invoke();
            return false;
        }

        public bool Play()
        {
            if (audioOutputDevice == null || audioOutputDevice.PlaybackState == PlaybackState.Stopped)
            {
                if (filePathList.Count > currentPlayIndex)
                {
                    beforePlayAction?.Invoke();
                    if (InitTitle(filePathList[currentPlayIndex]))
                    {
                        audioOutputDevice.Play();

                        UpdateProperties();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (audioOutputDevice.PlaybackState == PlaybackState.Paused)
            {
                audioOutputDevice.Play();
                afterResumeAction?.Invoke();

                return true;
            }

            return false;
        }

        public void Pause()
        {
            audioOutputDevice.Pause();
            UpdateProperties();

            afterPauseAction?.Invoke();
        }

        public void Stop()
        {
            audioOutputDevice?.Stop();
            afterStopAction?.Invoke();
        }

        public void KillAudio(bool reset = false)
        {
            if (audioOutputDevice != null)
            {
                audioOutputDevice.Dispose();
                audioOutputDevice = null;
            }

            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }

            if (reset)
            {
                currentPlayIndex = 0;
                filePathList.Clear();
            }

            UpdateProperties();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateProperties()
        {
            OnPropertyChanged(nameof(Playing));
            OnPropertyChanged(nameof(PlayingOrPaused));
            OnPropertyChanged(nameof(Stopped));
            OnPropertyChanged(nameof(Paused));
        }

        private bool InitTitle(string titlePath)
        {
            try
            {
                if (audioOutputDevice == null)
                {
                    audioOutputDevice = new WaveOutEvent();
                    audioOutputDevice.PlaybackStopped += OnAudioDevicePlaybackStopped;
                }

                audioFileReader = new AudioFileReader(titlePath);
                audioOutputDevice.Init(audioFileReader);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        // https://github.com/naudio/NAudio/blob/master/Docs/PlayAudioFileWinForms.md
        private void OnAudioDevicePlaybackStopped(object sender, StoppedEventArgs args)
        {
            currentPlayIndex++;

            if (filePathList.Count > currentPlayIndex)
            {
                if (InitTitle(filePathList[currentPlayIndex]))
                {
                    Play();
                }
            }
            else
            {
                Stop();
                KillAudio(reset: true);
            }

            UpdateProperties();
        }
    }
}