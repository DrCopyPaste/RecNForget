using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using NAudio.Wave;
using RecNForget.Services.Contracts;

namespace RecNForget.Services
{
    public class AudioPlaybackService : IAudioPlaybackService
    {
        private readonly Action queueErrorAction = null;
        private readonly List<string> filePathList = null;

        private WaveOutEvent audioOutputDevice = null;
        private AudioFileReader audioFileReader = null;
        private int currentPlayIndex = 0;

        public AudioPlaybackService()
        {
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

        public int ItemsCount
        {
            get
            {
                return filePathList.Count;
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
                UpdateProperties();

                return true;
            }

            UpdateProperties();

            return false;
        }

        public void Pause()
        {
            audioOutputDevice.Pause();
            UpdateProperties();
        }

        public void Stop()
        {
            audioOutputDevice?.Stop();
            KillAudio(reset: true);
            UpdateProperties();
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
            catch (Exception ex)
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
                Play();
            }
            else
            {
                Stop();
            }
        }
    }
}