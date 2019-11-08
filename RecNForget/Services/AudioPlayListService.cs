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
		private Action afterStopAction = null;

		private List<string> filePathList = null;
		private int currentPlayIndex = 0;

		public PlaybackState PlaybackState {
			get
			{
				return audioOutputDevice == null ? PlaybackState.Stopped : audioOutputDevice.PlaybackState;
			}
		}

		public AudioPlayListService(Action beforePlayAction, Action afterStopAction, Action afterPauseAction)
		{
			this.beforePlayAction = beforePlayAction;
			this.afterStopAction = afterStopAction;
			this.afterPauseAction = afterPauseAction;

			filePathList = new List<string>();
		}

		public void QueueFile(string filePath)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			if (fileInfo.Exists)
			{
				filePathList.Add(filePath);
			}
		}

		public void Play()
		{
			if (audioOutputDevice == null || audioOutputDevice.PlaybackState == PlaybackState.Stopped)
			{
				if (filePathList.Count > currentPlayIndex)
				{
					audioOutputDevice = new WaveOutEvent();
					audioOutputDevice.PlaybackStopped += OnAudioDevicePlaybackStopped;
					beforePlayAction?.Invoke();
					InitTitle(filePathList[currentPlayIndex]);

					audioOutputDevice.Play();
				}
			}
			else if (audioOutputDevice.PlaybackState == PlaybackState.Paused)
			{
				audioOutputDevice.Play();
			}
		}

		public void Pause()
		{
			audioOutputDevice.Pause();

			afterPauseAction?.Invoke();
		}

		private void InitTitle(string titlePath)
		{
			audioFileReader = new AudioFileReader(titlePath);
			audioOutputDevice.Init(audioFileReader);
		}

		// https://github.com/naudio/NAudio/blob/master/Docs/PlayAudioFileWinForms.md
		private void OnAudioDevicePlaybackStopped(object sender, StoppedEventArgs args)
		{
			currentPlayIndex++;

			audioFileReader.Dispose();
			audioFileReader = null;

			if (filePathList.Count > currentPlayIndex)
			{
				InitTitle(filePathList[currentPlayIndex]);
				audioOutputDevice.Play();
			}
			else
			{
				audioOutputDevice.Dispose();
				audioOutputDevice = null;

				afterStopAction?.Invoke();
			}
		}
	}
}