# RecNForget
Recording should be as easy as pressing a button.

# How to use
- RecNForget sets the recording hotkey to [Pause] by default
- press this button to start recording
- press it again to stop recording and save your file
- output folder and hotkey can be changed in the settings menu

For more detailed help refer to the [online help pages](https://github.com/DrCopyPaste/RecNForget/blob/master/Help/toc.md).

# Installation
Run msi-file from the [latest release](https://github.com/DrCopyPaste/RecNForget/releases/latest) to install.

(RecNForget requires [Microsoft .Net Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) to run.)

# The code
Feel free to download the code and play with it.

For stable and buildable code please refer to the version tagged commits, in between releases there may very well be errors in the code.

- Currently I use [Microsoft's Visual Studio 2019 Community](https://visualstudio.microsoft.com/vs/community/) for development.
- To build the setup project you need to have the [WiX Toolset](https://wixtoolset.org/releases/v3.11.2/stable) installed.

# Version
latest stable release: [0.3](https://github.com/DrCopyPaste/RecNForget/releases/latest).

- Currently developing: [0.4](https://github.com/DrCopyPaste/RecNForget/projects/2).
- ![CI](https://github.com/DrCopyPaste/RecNForget/workflows/CI/badge.svg)

KEEP IN MIND: WORK IN PROGRESS MAY BE UNSTABLE! :D

But, if you feel extra keen to try out the latest, you can
- go [here](https://github.com/DrCopyPaste/RecNForget/actions?query=workflow%3ACI+is%3Asuccess++)
- click on the latest build
- download the artifact "RecNForget_TestBuild" that contains generated setup file
(downloading is currently only allowed for github users it seems)

# upcoming features in next release
- migrated to .NET Framework 4.8

- FIX: uninstalling RecNForget will now also delete all application data (under %APPDATA% machine configuration and in registry for autostart)
- FIX: clicking the open folder button does not crash RecNForget anymore, if that folder does not (yet) exist
- FIX: changing the file prefix changes the displayed file name with output path

- default hotkey for "toggle record" is now [Pause]
- shrinked main window in size and reorganized controls a bit
- all action buttons now are in the same row at the bottom
- no more standard title bar
- added icons for most buttons
- can select the slected file in the explorer using [Down-Arrow]-key in the main window
- can play/pause slected file using [Space]-key in the main window
- added skip buttons to navigate through output folder (auto selects the latest file on program start)
- can skip to previous/next file using [Left-Arrow]/[Right-Arrow]-key in the main window
- can rename selected file via pencil icon next to "selected file"
- can rename slected file using [Return]-key in the main window
- can delete selected file via trash can icon next to "selected file"
- can delete selected file using [Delete]-key in the main window
- window options and settings can be opened via context menu on the main window
- file name pattern textbox no longer in main window (it is instead configured by the pencil button next to "recording path" or via settings menu)
- can configure target path via folder icon next to "recording path"
- consistently use white background for (almost) all controls
- auto selecting the last recording is now optional (enabled by default)
- added a help link to the menu (opens help window)
- Added a stop-replay button (this also closes the file in RecNForget enabling you to move or rename it...)
- can stop playing slected file using [Esc]-key in the main window
- clicking the open folder button now selects the last result file in explorer (or just opens folder if there is now result yet)
- Taskbar Icon background turns green when replaying the last audio (stays green also when paused to remind you that the file is still open)
- tray icon menu now shows the same menu as the windows options menu at the top left of the main window
- when started for the first time, RecNForget will show the toggle record and output folder setting and allow to change them
- RecNForget can now update itself via clicking the about-button (via top left window button) and then "check for updates"
- RecNForget can now show random tips on program start (did you know?) (enabled by default)
- Installation is cancelled if .Net framework is not found
- Installer lets user choose target installation folder
- Installer now shows a minimal UI with license agreement and logo during installation
- Installer now starts with elevated privileges (so it can update the files installed in programs folder)

# Features missing for version 1.0
- allow toggling input source (default output, or maybe mic/something else)?
- show record icon (red circle) in bottom right corner when recording (configurable)
- (meaning something always on top other than the main window showing recording details (recording time/buffersize etc)
- clean up source code for maintainability

# Copyright and License
RecNForget is written in C# using .NET Framework 4.8 and Microsoft WPF

Code by DrCopyPaste [github.com/DrCopyPaste/RecNForget](https://github.com/DrCopyPaste/RecNForget)

Logo by DrCopyPaste (using icons from onlinewebfonts.com - see below)

Beep sounds by a cheap program from DrCopyPaste with love.

Click sounds by [lit-audio@gmx.de](mailto:lit-audio@gmx.de)/[soundcloud.com/wolfgankh](https://soundcloud.com/wolfgankh)



The following libaries are used by RecNForget:

[FMUtils.KeyboardHook.1.0.140.2145](https://github.com/factormystic/FMUtils.KeyboardHook#readme) Copyright (c) Factor Mystic - [License](https://github.com/factormystic/FMUtils.KeyboardHook/blob/master/license.txt)

[Hardcodet.NotifyIcon.Wpf.1.0.8](http://www.hardcodet.net/wpf-notifyicon) Copyright (c) Philipp Sumi - [CPOL 1.02](https://www.codeproject.com/info/cpol10.aspx)

[NAudio.1.10.0](https://github.com/naudio/NAudio) Copyright (c) Mark Heath & Contributors - [Ms-PL](https://github.com/naudio/NAudio/blob/master/license.txt)

[Nerdbank.GitVersioning.3.0.50](https://github.com/aarnott/Nerdbank.GitVersioning) Copyright (c) Andrew Arnott - [MIT](https://licenses.nuget.org/MIT)

[Newtonsoft.Json.12.0.3](https://www.newtonsoft.com/json) Copyright (c) James Newton-King - [MIT](https://licenses.nuget.org/MIT)

[Octokit.0.40.0](https://github.com/octokit/octokit.net) Copyright (c) GitHub - [MIT](https://licenses.nuget.org/MIT)

[Ookii.Dialogs.Wpf.1.1.0](https://github.com/caioproiete/ookii-dialogs-wpf) Copyright (c) Sven Groot,Caio Proiete - [License](https://github.com/caioproiete/ookii-dialogs-wpf/blob/master/LICENSE)

[WiX Toolset build tools v3.11.2.4516](https://wixtoolset.org/) Copyright (c) .NET Foundation - [MS-RL](https://wixtoolset.org/about/license/)



The following images were used to create RecNForget:

accept_icon518267.black.png was generated from [onlinewebfonts.com/icon/518267](https://www.onlinewebfonts.com/icon/518267)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

accept_icon518267.black.white.png was generated from [onlinewebfonts.com/icon/518267](https://www.onlinewebfonts.com/icon/518267)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

accept_icon518267.svg was generated from [onlinewebfonts.com/icon/518267](https://www.onlinewebfonts.com/icon/518267)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

bugfix_icon140609.black.png was generated from [onlinewebfonts.com/icon/140609](https://www.onlinewebfonts.com/icon/140609)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

bugfix_icon140609.svg was generated from [onlinewebfonts.com/icon/140609](https://www.onlinewebfonts.com/icon/140609)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

cancel_icon428740.black.png was generated from [onlinewebfonts.com/icon/428740](https://www.onlinewebfonts.com/icon/428740)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

cancel_icon428740.svg was generated from [onlinewebfonts.com/icon/428740](https://www.onlinewebfonts.com/icon/428740)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

close_icon211720.black.png was generated from [onlinewebfonts.com/icon/211720](https://www.onlinewebfonts.com/icon/211720)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

close_icon211720.svg was generated from [onlinewebfonts.com/icon/211720](https://www.onlinewebfonts.com/icon/211720)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

close_icon349388.black.png was generated from [onlinewebfonts.com/icon/349388](https://www.onlinewebfonts.com/icon/349388)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

close_icon349388.svg was generated from [onlinewebfonts.com/icon/349388](https://www.onlinewebfonts.com/icon/349388)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

download_icon602.black.png was generated from [onlinewebfonts.com/icon/602](https://www.onlinewebfonts.com/icon/602)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

download_icon602.svg was generated from [onlinewebfonts.com/icon/602](https://www.onlinewebfonts.com/icon/602)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

editSetting_icon451737.black.png was generated from [onlinewebfonts.com/icon/451737](https://www.onlinewebfonts.com/icon/451737)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

editSetting_icon451737.svg was generated from [onlinewebfonts.com/icon/451737](https://www.onlinewebfonts.com/icon/451737)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

folder_icon122592.black.png was generated from [onlinewebfonts.com/icon/122592](https://www.onlinewebfonts.com/icon/122592)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

folder_icon122592.svg was generated from [onlinewebfonts.com/icon/122592](https://www.onlinewebfonts.com/icon/122592)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

folder_icon17782.black.png was generated from [onlinewebfonts.com/icon/17782](https://www.onlinewebfonts.com/icon/17782)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

folder_icon17782.svg was generated from [onlinewebfonts.com/icon/17782](https://www.onlinewebfonts.com/icon/17782)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

folder_icon17783.black.png was generated from [onlinewebfonts.com/icon/17783](https://www.onlinewebfonts.com/icon/17783)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

folder_icon17783.svg was generated from [onlinewebfonts.com/icon/17783](https://www.onlinewebfonts.com/icon/17783)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

help_icon544157.black.png was generated from [onlinewebfonts.com/icon/544157](https://www.onlinewebfonts.com/icon/544157)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

help_icon544157.svg was generated from [onlinewebfonts.com/icon/544157](https://www.onlinewebfonts.com/icon/544157)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

information_icon2132.black.png was generated from [onlinewebfonts.com/icon/2132](https://www.onlinewebfonts.com/icon/2132)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

information_icon2132.svg was generated from [onlinewebfonts.com/icon/2132](https://www.onlinewebfonts.com/icon/2132)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

information_icon280732.black.png was generated from [onlinewebfonts.com/icon/280732](https://www.onlinewebfonts.com/icon/280732)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

information_icon280732.svg was generated from [onlinewebfonts.com/icon/280732](https://www.onlinewebfonts.com/icon/280732)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

lightbulb_icon372420.black.png was generated from [onlinewebfonts.com/icon/372420](https://www.onlinewebfonts.com/icon/372420)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

lightbulb_icon372420.svg was generated from [onlinewebfonts.com/icon/372420](https://www.onlinewebfonts.com/icon/372420)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

message_error_icon304479.black.png was generated from [onlinewebfonts.com/icon/304479](https://www.onlinewebfonts.com/icon/304479)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

message_error_icon304479.svg was generated from [onlinewebfonts.com/icon/304479](https://www.onlinewebfonts.com/icon/304479)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

message_info_icon277439.black.png was generated from [onlinewebfonts.com/icon/277439](https://www.onlinewebfonts.com/icon/277439)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

message_info_icon277439.svg was generated from [onlinewebfonts.com/icon/277439](https://www.onlinewebfonts.com/icon/277439)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

message_question_icon109120.black.png was generated from [onlinewebfonts.com/icon/109120](https://www.onlinewebfonts.com/icon/109120)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

message_question_icon109120.svg was generated from [onlinewebfonts.com/icon/109120](https://www.onlinewebfonts.com/icon/109120)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

minimize_icon246566.black.png was generated from [onlinewebfonts.com/icon/246566](https://www.onlinewebfonts.com/icon/246566)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

minimize_icon246566.svg was generated from [onlinewebfonts.com/icon/246566](https://www.onlinewebfonts.com/icon/246566)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

openFolder_icon192688.black.png was generated from [onlinewebfonts.com/icon/192688](https://www.onlinewebfonts.com/icon/192688)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

openFolder_icon192688.svg was generated from [onlinewebfonts.com/icon/192688](https://www.onlinewebfonts.com/icon/192688)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

openFolder_icon356582.black.png was generated from [onlinewebfonts.com/icon/356582](https://www.onlinewebfonts.com/icon/356582)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

openFolder_icon356582.svg was generated from [onlinewebfonts.com/icon/356582](https://www.onlinewebfonts.com/icon/356582)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

pencil_icon353603.black.png was generated from [onlinewebfonts.com/icon/353603](https://www.onlinewebfonts.com/icon/353603)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

pencil_icon353603.svg was generated from [onlinewebfonts.com/icon/353603](https://www.onlinewebfonts.com/icon/353603)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

pencil_icon421491.black.png was generated from [onlinewebfonts.com/icon/421491](https://www.onlinewebfonts.com/icon/421491)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

pencil_icon421491.svg was generated from [onlinewebfonts.com/icon/421491](https://www.onlinewebfonts.com/icon/421491)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

pencil_icon79516.black.png was generated from [onlinewebfonts.com/icon/79516](https://www.onlinewebfonts.com/icon/79516)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

pencil_icon79516.svg was generated from [onlinewebfonts.com/icon/79516](https://www.onlinewebfonts.com/icon/79516)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

plus_icon171498.black.png was generated from [onlinewebfonts.com/icon/171498](https://www.onlinewebfonts.com/icon/171498)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

plus_icon171498.svg was generated from [onlinewebfonts.com/icon/171498](https://www.onlinewebfonts.com/icon/171498)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

settings_icon40863.black.png was generated from [onlinewebfonts.com/icon/40863](https://www.onlinewebfonts.com/icon/40863)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

settings_icon40863.svg was generated from [onlinewebfonts.com/icon/40863](https://www.onlinewebfonts.com/icon/40863)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

settings_icon488154.black.png was generated from [onlinewebfonts.com/icon/488154](https://www.onlinewebfonts.com/icon/488154)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

settings_icon488154.svg was generated from [onlinewebfonts.com/icon/488154](https://www.onlinewebfonts.com/icon/488154)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

settings_icon522890.black.png was generated from [onlinewebfonts.com/icon/522890](https://www.onlinewebfonts.com/icon/522890)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

settings_icon522890.svg was generated from [onlinewebfonts.com/icon/522890](https://www.onlinewebfonts.com/icon/522890)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

shutDown_icon170336.black.png was generated from [onlinewebfonts.com/icon/170336](https://www.onlinewebfonts.com/icon/170336)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

shutDown_icon170336.svg was generated from [onlinewebfonts.com/icon/170336](https://www.onlinewebfonts.com/icon/170336)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

tools_icon536263.black.png was generated from [onlinewebfonts.com/icon/536263](https://www.onlinewebfonts.com/icon/536263)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

tools_icon536263.svg was generated from [onlinewebfonts.com/icon/536263](https://www.onlinewebfonts.com/icon/536263)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

trash_icon77730.black.png was generated from [onlinewebfonts.com/icon/77730](https://www.onlinewebfonts.com/icon/77730)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

trash_icon77730.svg was generated from [onlinewebfonts.com/icon/77730](https://www.onlinewebfonts.com/icon/77730)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

windowOptions_icon345920.black.png was generated from [onlinewebfonts.com/icon/345920](https://www.onlinewebfonts.com/icon/345920)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

windowOptions_icon345920.svg was generated from [onlinewebfonts.com/icon/345920](https://www.onlinewebfonts.com/icon/345920)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

windowOptions_icon510828.black.png was generated from [onlinewebfonts.com/icon/510828](https://www.onlinewebfonts.com/icon/510828)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

windowOptions_icon510828.svg was generated from [onlinewebfonts.com/icon/510828](https://www.onlinewebfonts.com/icon/510828)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

windowSettings_icon248857.black.png was generated from [onlinewebfonts.com/icon/248857](https://www.onlinewebfonts.com/icon/248857)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

windowSettings_icon248857.svg was generated from [onlinewebfonts.com/icon/248857](https://www.onlinewebfonts.com/icon/248857)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

WixUI-logo.svg was generated from [onlinewebfonts.com/icon/55290](https://www.onlinewebfonts.com/icon/55290) and [onlinewebfonts.com/icon/715](https://www.onlinewebfonts.com/icon/715)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

WixUIBannerBmp.bmp was generated from [onlinewebfonts.com/icon/55290](https://www.onlinewebfonts.com/icon/55290) and [onlinewebfonts.com/icon/715](https://www.onlinewebfonts.com/icon/715)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

WixUIBannerBmp.png was generated from [onlinewebfonts.com/icon/55290](https://www.onlinewebfonts.com/icon/55290) and [onlinewebfonts.com/icon/715](https://www.onlinewebfonts.com/icon/715)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

WixUIDialogBmp.bmp was generated from [onlinewebfonts.com/icon/55290](https://www.onlinewebfonts.com/icon/55290) and [onlinewebfonts.com/icon/715](https://www.onlinewebfonts.com/icon/715)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

WixUIDialogBmp.png was generated from [onlinewebfonts.com/icon/55290](https://www.onlinewebfonts.com/icon/55290) and [onlinewebfonts.com/icon/715](https://www.onlinewebfonts.com/icon/715)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

logo.ico was generated from [onlinewebfonts.com/icon/55290](https://www.onlinewebfonts.com/icon/55290) and [onlinewebfonts.com/icon/715](https://www.onlinewebfonts.com/icon/715)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

logo.svg was generated from [onlinewebfonts.com/icon/55290](https://www.onlinewebfonts.com/icon/55290) and [onlinewebfonts.com/icon/715](https://www.onlinewebfonts.com/icon/715)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

logo_big.png was generated from [onlinewebfonts.com/icon/55290](https://www.onlinewebfonts.com/icon/55290) and [onlinewebfonts.com/icon/715](https://www.onlinewebfonts.com/icon/715)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

logo_small.png was generated from [onlinewebfonts.com/icon/55290](https://www.onlinewebfonts.com/icon/55290) and [onlinewebfonts.com/icon/715](https://www.onlinewebfonts.com/icon/715)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

