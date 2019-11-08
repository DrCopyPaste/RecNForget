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
		private Action queueErrorAction = null;

		private List<string> filePathList = null;
		private int currentPlayIndex = 0;

		public PlaybackState PlaybackState {
			get
			{
				return audioOutputDevice == null ? PlaybackState.Stopped : audioOutputDevice.PlaybackState;
			}
		}

		public AudioPlayListService(Action beforePlayAction = null, Action afterStopAction = null, Action afterPauseAction = null, Action queueErrorAction = null)
		{
			this.beforePlayAction = beforePlayAction;
			this.afterStopAction = afterStopAction;
			this.afterPauseAction = afterPauseAction;
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

		public void Play()
		{
			if (audioOutputDevice == null || audioOutputDevice.PlaybackState == PlaybackState.Stopped)
			{
				if (filePathList.Count > currentPlayIndex)
				{
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

		public void Stop()
		{
			audioOutputDevice?.Stop();
		}

		public void KillAudio(bool reset = false)
		{
			Stop();

			if (audioOutputDevice != null)
			{
				audioOutputDevice.Dispose();
				audioOutputDevice = null;
			}

			if (audioOutputDevice != null)
			{
				audioOutputDevice.Dispose();
				audioOutputDevice = null;
			}

			if (reset)
			{
				currentPlayIndex = 0;
				filePathList.Clear();
				afterStopAction?.Invoke();
			}
		}

		private void InitTitle(string titlePath)
		{
			if (audioOutputDevice == null)
			{
				audioOutputDevice = new WaveOutEvent();
				audioOutputDevice.PlaybackStopped += OnAudioDevicePlaybackStopped;
			}

			audioFileReader = new AudioFileReader(titlePath);
			audioOutputDevice.Init(audioFileReader);
		}

		// https://github.com/naudio/NAudio/blob/master/Docs/PlayAudioFileWinForms.md
		private void OnAudioDevicePlaybackStopped(object sender, StoppedEventArgs args)
		{
			currentPlayIndex++;

			if (filePathList.Count > currentPlayIndex)
			{
				KillAudio(reset: false);
				InitTitle(filePathList[currentPlayIndex]);
				Play();
			}
			else
			{
				KillAudio(reset: true);
			}
		}
	}
}