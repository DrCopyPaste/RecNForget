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
	public class HotkeyService
	{
		private bool hotkeyWaitingForRelease;
		private bool paused;

		private Action startRecordingAction;
		private Action stopRecordingAction;

		// will be newly instantiated every time record starting is triggered
		private static WasapiLoopbackCapture captureInstance;
		private Hook keyboardHook;

		public string CurrentFileName { get; set; } = string.Empty;
		public string LastFileName { get; set; } = string.Empty;

		public bool CurrentlyRecording { get; set; }


		public HotkeyService(Action startRecordingAction, Action stopRecordingAction)
		{
			keyboardHook = new Hook("Global Action Hook");
			keyboardHook.KeyDownEvent = KeyDown;
			keyboardHook.KeyUpEvent = KeyUp;

			this.startRecordingAction = startRecordingAction;
			this.stopRecordingAction = stopRecordingAction;

			CurrentlyRecording = false;
			hotkeyWaitingForRelease = false;


			paused = false;
		}

		private void KeyDown(KeyboardHookEventArgs e)
		{
			if (!this.paused)
			{
				string keyAsString = HotkeyToStringTranslator.GetKeyboardHookEventArgsAsString(e);

				var HotKey_StartStopRecording = HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_StartStopRecording");
				var HotKey_OpenLastRecording = HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_OpenLastRecording");
				var HotKey_OpenOutputPath = HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_OpenOutputPath");
				var HotKey_SetFileNamePrefix = HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_SetFileNamePrefix");
				var HotKey_ToggleFileNamePromptMode = HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_ToggleFileNamePromptMode");
				var HotKey_SetOutputPath = HotkeyToStringTranslator.GetHotkeySettingAsString("HotKey_SetOutputPath");

				if (keyAsString == HotKey_StartStopRecording)
				{
					hotkeyWaitingForRelease = true;
				}
			}
		}

		private void KeyUp(KeyboardHookEventArgs e)
		{
			if (!this.paused)
			{
				// would be nice to check here that hotkey was indeed released (what keys where lifted)
				// unfortunately this is not supported by the library atm (also no support for more than one hotkey excluding modifier keys)
				if (hotkeyWaitingForRelease)
				{
					hotkeyWaitingForRelease = false;

					if (CurrentlyRecording)
					{
						StopRecording();
					}
					else
					{
						StartRecording();
					}
				}
			}
		}

		public void PauseCapturingHotkeys(bool pause = true)
		{
			this.paused = pause;
		}

		public void ResumeCapturingHotkeys()
		{
			PauseCapturingHotkeys(false);
		}

		public void StartRecording()
		{
			CurrentlyRecording = true;

			var settingOutputPath = System.Configuration.ConfigurationManager.AppSettings["OutputPath"];
			var settingFilenamePrefix = System.Configuration.ConfigurationManager.AppSettings["FilenamePrefix"];

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
