# RecNForget 0.2
Set a global Windows hotkey to capture and save default audio output to wave files

With RecNForget running, capturing audio should be as easy as:
- press "Recording"-hotkey to start recording
- do your audio thing...
- press "Recording"-hotkey again to finish the recording

Your recorded audio is now saved to an automatically generated file in RecNForget's output folder.

# How to install
RecNForget requires .Net Framework 4.7 to run.

You should be able to just extract RecNForget from the latest release and run the program.
(https://github.com/DrCopyPaste/RecNForget/releases)

# How to Use
- by default RecNForget sets [Ctrl] + [F12] as the Record hotkey. (changeable in settings)
- have RecNForget running
- press the configured Record hotkey
- start the audio which you want to capure
- let the audio play
- to stop captureing press the configured Record hotkey again
- the file will be saved to the configured output folder (shown to you in main menu, changeable in settings)
- file names will be generated using a file name prefix that you can specify plus a timestamp of the start time of recording
- you can replay the last recorded file from the running session

# upcoming features in next version
(commited changes not yet released)
- FIX error when trying to play the last file (but it was moved or deleted - missing replay file leads to button being disabled, missing audio feedback files are silently ignored for now)
- FIX: application icon was not shown when executing from a directory different to the application directory
- FIX: suppress not allowed characters for filenames in filename prefix pattern

- can enable instant auto-replay of audio after recording (disabled by default)
- can enable short audio beep as feedback marking beginning and end of recording (enabled by default)
- can enable short audio beep as feedback marking beginning and end of replayed audio (disabled by default)
- can enable balloon tips (toast messages) for when recording starts or stops (enabled by default)
- can enable running main window as a background process (minimized to tray, disabled by default)
- can set main window to be always on top (disabled by default)
- added auto-start-option (disabled by default)

- can click balloon top when recording finished to select recorded file in explorer
- can close application via tray icon context menu
- show length of recording in main window and balloon tips

# Features missing for version 1.0
FIX: not all settings from settings window take effect immediately some require restart

- show record (red circle) in bottom right corner when recording (configurable)
- different peep sound for replay start/stop
- clean up source code for maintainability
- rework visual layout of windows (more consistency and maybe smaller nicer windwos)
- have an (Un-)Installer and Updater to set default paths, registry entries for uninstalling and preserving custom settings when updating

# Nice to haves in future releases
- suppress not allowed characters for filenames
- allow disabling some hotkeys explicitly (decreasing the amount of hotkeys, the listener has to listen to)
- include how-to-build-file (maybe batch with msbuild)
- icons for buttons
- RecNForget can self-update (possibly querying github's releases page)
- "gimmick" mode (allow using application as a sound board, binding hotkeys to different sound outputs, maybe even allow changing the output device they are output to (to insert sounds into a running mic session for example))
