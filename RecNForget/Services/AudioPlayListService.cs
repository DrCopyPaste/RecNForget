using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RecNForget.Services
{
	public class AudioPlayListService
	{
		private WaveOutEvent audioOutputDevice = null;
		private AudioFileReader audioFileReader = null;

		private Action beforePlayAction = null;
		private Action afterPauseAction = null;
        private Action afterResumeAction = null;
        private Action afterStopAction = null;
		private Action queueErrorAction = null;

		private List<string> filePathList = null;
		private int currentPlayIndex = 0;

		public PlaybackState PlaybackState {
			get
			{
				return audioOutputDevice == null ? PlaybackState.Stopped : audioOutputDevice.PlaybackState;
			}
		}

		public AudioPlayListService(Action beforePlayAction = null, Action afterStopAction = null, Action afterPauseAction = null, Action afterResumeAction = null, Action queueErrorAction = null)
		{
			this.beforePlayAction = beforePlayAction;
			this.afterStopAction = afterStopAction;
			this.afterPauseAction = afterPauseAction;
            this.afterResumeAction = afterResumeAction;
            this.queueErrorAction = queueErrorAction;

			filePathList = new List<string>();
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
			catch (Exception e)
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
		}
	}
}