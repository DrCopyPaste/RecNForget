# RecNForget 0.3
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

# Checking out the code

Feel free to download the code and play with it, for now this has mainly prototype quality, but I try to clean up the code as I go.
Please keep in mind this is my first (and very maybe last) WPF application so this might make it a bit ore messy.

For stable and buildable code please refer to the version tagged commits, in between releases there may very well be errors in the code ;)

# upcoming features in next version (commited, not yet released)
- Installer now starts with elevated privileges (so it can update the files installed in programs folder)
- installing WiX toolset locally is no longer required to build the setup project


# Features missing for version 1.0
- show record (red circle) in bottom right corner when recording (configurable)
- clean up source code for maintainability
- rework visual layout of windows (more consistency and maybe smaller nicer windows)
- clean up registry and user app data on uninstall

# Nice to haves in future releases
- suppress not allowed characters for filenames
- allow disabling some hotkeys explicitly (decreasing the amount of hotkeys, the listener has to listen to)
- include how-to-build-file (maybe batch with msbuild)
- icons for buttons
- RecNForget can self-update (possibly querying github's releases page)
- "gimmick" mode (allow using application as a sound board, binding hotkeys to different sound outputs, maybe even allow changing the output device they are output to (to insert sounds into a running mic session for example))

# Credits
Code by DrCopyPaste

Click sounds for replay by https://soundcloud.com/slutski (litw@gmx.de)
