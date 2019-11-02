# RecNForget 0.2
Set a global Windows hotkey to capture and save default audio output to wave files

# How to install
RecNForget requires .Net Framework 4.7 to run.
You should be able to just extract the application from the latest release and run the program.

# How to Use
- by default RecNForget sets [Ctrl] + [F12] as the Record hotkey. (changeable in settings)
- aave RecNForget running
- press the configured Record hotkey
- start the audio which you want to capure
- let the audio play
- to stop captureing press the configured Record hotkey again
- the file will be saved to the configured output folder (shown to you in main menu, changeable in settings)
- file names will be generated using a file name prefix that you can specify plus a timestamp of the start time of recording
- you can replay the last recorded file from the running session

# Features missing for version 1.0
- show record (red circle) in bottom right corner when recording (configurable)
- have always on top option for the main window (configurable)
- show balloon tips (configurable)
- show generated file in file browser (configurable)
- peep sound for recording start/stop (configurable)
- instant replay recorded audio? (configurable)
- different peep sound for replay start/stop (configurable)
- allow disabling some hotkeys explicitly (decreasing the amount of hotkeys, the listener has to listen to)

# Nice to haves in future releases
- program can run in background as a windows process listening to keystrokes
(maybe only allow if some visual or audio feedback for start/stop recording is active - to not make this a spyware)
- suppress not allowed characters for filenames
- include how-to-build-file (maybe batch with msbuild)
- icons for buttons
