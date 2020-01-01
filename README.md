# RecNForget 0.3
Set a global Windows hotkey to capture and save default audio output to wave files

Current Version is 0.3.

Progress towards 0.4-Release can be tracked [here](https://github.com/DrCopyPaste/RecNForget/projects/2).

Recording audio should be as easy as pushing a button:
- press "Toggle Record"-hotkey to start recording
- do your audio thing...
- press "Toggle Record"-hotkey again to finish the recording

Your recorded audio is now saved to an automatically generated file in RecNForget's output folder.

# How to install
RecNForget requires .Net Framework 4.7 to run.

Run msi-file from the [latest release](https://github.com/DrCopyPaste/RecNForget/releases/latest) to install.

# How to Use
- by default RecNForget sets [Pause] as the Record hotkey. (changeable in settings)
- have RecNForget running
- press the configured Record hotkey
- start the audio which you want to capure
- let the audio play
- to stop capturing press the configured Record hotkey again
- the file will be saved to the configured output folder (shown to you in main menu, changeable in settings)
- file names will be generated using a file name prefix that you can specify plus a timestamp of the start time of recording
- you can replay the last recorded file from the running session

# Cloning and playing with the code
Feel free to download the code and play with it, for now this has mainly prototype quality, but I try to clean up the code as I go.

Currently I use Microsoft's Visual Studio Community 2019 for development.
To build the setup project you need to have the [WiX Toolset](https://wixtoolset.org/releases/v3.11.2/stable) installed.

Please keep in mind this is my first (and very maybe last) WPF application so this might make it a bit more messy.
For stable and buildable code please refer to the version tagged commits, in between releases there may very well be errors in the code ;)

# upcoming features in next version (commited, not yet released)
- FIX: uninstalling RecNForget will now also delete all application data (under %APPDATA% machine configuration and in registry for autostart)
- FIX: clicking the open folder button does not crash RecNForget anymore, if that folder does not (yet) exist
- FIX: changing the file prefix changes the displayed file name with output path

- changed default hotkey for "toggle record" to [Pause]
- shrinked main window in size and reorganized controls a bit
- all action buttons now are in the same row at the bottom
- added skip buttons to navigate through output folder (auto selects the latest file on program start)
- can rename selected file via pencil icon next to "selected file"
- can delete selected file via trash can icon next to "selected file"
- file name pattern no longer in main window (it is instead configured by the pencil button next to "recording path" or via settings menu)
- can configure target path via folder icon next to "recording path"
- consistently use white background for (almost) all controls
- auto selecting the last recording is now optional (enabled by default)
- no more standard title bar
- added icons for most buttons
- added a help link to the menu (calls link in browser going to THIS readme.md, I will replace this with an in-application help later on)
- Added a stop-replay button (this also closes the file in RecNForget enabling you to move or rename it...)
- clicking the open folder button now selects the last result file in explorer (or just opens folder if there is now result yet)
- Taskbar Icon background turns green when replaying the last audio (stays green also when paused to remind you that the file is still open)
- RecNForget can now update itself via clicking the about-button (via top left window button) and then "check for updates"
- Installation is cancelled if .Net framework is not found
- Installer lets user choose target installation folder
- Intaller now shows a minimal UI with license agreement and logo during installation
- Installer now starts with elevated privileges (so it can update the files installed in programs folder)

# Features missing for version 1.0
- allow toggling input source (default output, or maybe mic/something else)?
- show record (red circle) in bottom right corner when recording (configurable)
- clean up source code for maintainability

# Credits
- Code by [DrCopyPaste](https://github.com/DrCopyPaste)
- Logo by [DrCopyPaste](https://github.com/DrCopyPaste)
- Logo source svgs from https://www.onlinewebfonts.com/icon/55290 and https://www.onlinewebfonts.com/icon/715
- Click sounds for replay by https://soundcloud.com/slutski (litw@gmx.de)
