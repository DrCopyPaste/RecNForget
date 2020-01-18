# RecNForget
Recording should be as easy as pressing a button.

# How to use
- RecNForget sets the recording hotkey to [Pause] by default
- just press this button to start recording
- and press it again to stop recording and save your file
- output folder and hotkey can be changed in the settings menu

For more detailed help please refer to the [help pages](https://github.com/DrCopyPaste/RecNForget/blob/master/Help/toc.md).

# Installation
Run msi-file from the [latest release](https://github.com/DrCopyPaste/RecNForget/releases/latest) to install.

(RecNForget requires [Microsoft .Net Framework 4.7](https://dotnet.microsoft.com/download/dotnet-framework/net47) to run.)

# The code
Feel free to download the code and play with it.

Please keep in mind this is my very first WPF application. To be completely honest, development so far is primarily feature-driven so do not visit the code looking for best practices :)

For stable and buildable code please refer to the version tagged commits, in between releases there may very well be errors in the code.

- Currently I use [Microsoft's Visual Studio 2019 Community](https://visualstudio.microsoft.com/vs/community/) for development.
- To build the setup project you need to have the [WiX Toolset](https://wixtoolset.org/releases/v3.11.2/stable) installed.

# This version
The curretly released version is 0.3. Progress towards the next 0.4-Release can be tracked [here](https://github.com/DrCopyPaste/RecNForget/projects/2).

# upcoming features in next release
- FIX: uninstalling RecNForget will now also delete all application data (under %APPDATA% machine configuration and in registry for autostart)
- FIX: clicking the open folder button does not crash RecNForget anymore, if that folder does not (yet) exist
- FIX: changing the file prefix changes the displayed file name with output path

- changed default hotkey for "toggle record" to [Pause]
- shrinked main window in size and reorganized controls a bit
- can select the slected file in the explorer using [Down-Arrow]-key in the main window
- all action buttons now are in the same row at the bottom
- can play/pause slected file using [Space]-key in the main window
- added skip buttons to navigate through output folder (auto selects the latest file on program start)
- can skip to previous/next file using [Left-Arrow]/[Right-Arrow]-key in the main window
- can rename selected file via pencil icon next to "selected file"
- can rename slected file using [Return]-key in the main window
- can delete selected file via trash can icon next to "selected file"
- can delete selected file using [Delete]-key in the main window
- window options and settings can be opened via context menu on the main window
- file name pattern no longer in main window (it is instead configured by the pencil button next to "recording path" or via settings menu)
- can configure target path via folder icon next to "recording path"
- consistently use white background for (almost) all controls
- auto selecting the last recording is now optional (enabled by default)
- no more standard title bar
- added icons for most buttons
- added a help link to the menu (calls link in browser going to THIS readme.md, I will replace this with an in-application help later on)
- Added a stop-replay button (this also closes the file in RecNForget enabling you to move or rename it...)
- can stop playing slected file using [Esc]-key in the main window
- clicking the open folder button now selects the last result file in explorer (or just opens folder if there is now result yet)
- Taskbar Icon background turns green when replaying the last audio (stays green also when paused to remind you that the file is still open)
- RecNForget can now update itself via clicking the about-button (via top left window button) and then "check for updates"
- Installation is cancelled if .Net framework is not found
- Installer lets user choose target installation folder
- Intaller now shows a minimal UI with license agreement and logo during installation
- Installer now starts with elevated privileges (so it can update the files installed in programs folder)

# Features missing for version 1.0
- allow toggling input source (default output, or maybe mic/something else)?
- show record icon (red circle) in bottom right corner when recording (configurable)
- clean up source code for maintainability
- and probably a bunch of other stuff :D

# Credits
- Code by [DrCopyPaste](https://github.com/DrCopyPaste)
- Logo by [DrCopyPaste](https://github.com/DrCopyPaste)
- Logo source svgs from https://www.onlinewebfonts.com/icon/55290 and https://www.onlinewebfonts.com/icon/715
- Click sounds for replay by [wolfgankh](https://soundcloud.com/wolfgankh) (litw@gmx.de)
- All Button images use icons taken from [onlinewebfonts.com](https://www.onlinewebfonts.com)
