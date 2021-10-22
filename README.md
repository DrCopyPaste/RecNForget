﻿# RecNForget
Recording should be as easy as pressing a button.

# How to use
- RecNForget sets the recording hotkey to [Pause] by default
- press this button to start recording
- press it again to stop recording and save your file
- output folder and hotkey can be changed in the settings menu

For more detailed help refer to the [online help pages](https://github.com/DrCopyPaste/RecNForget/blob/master/Help/toc.md).

# Installation
Be sure to have [Microsoft .Net Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) installed.

Run msi-file from the [latest release](https://github.com/DrCopyPaste/RecNForget/releases/latest) to install.

# The code
Feel free to download the code and play with it.

For stable and buildable code please refer to the version tagged commits, in between releases there may very well be errors in the code.

- Currently I use [Microsoft's Visual Studio 2019 Community](https://visualstudio.microsoft.com/vs/community/) for development.
- To build the setup project you need to have the [WiX Toolset](https://wixtoolset.org/releases/v3.11.2/stable) installed.

# Version
latest stable release: [0.4](https://github.com/DrCopyPaste/RecNForget/releases/latest).

- Currently developing: [0.6](https://github.com/DrCopyPaste/RecNForget/projects/4).
- ![CI](https://github.com/DrCopyPaste/RecNForget/workflows/CI/badge.svg)

KEEP IN MIND: WORK IN PROGRESS MAY BE UNSTABLE! :D

But, if you feel extra keen to try out the latest, you can
- go [here](https://github.com/DrCopyPaste/RecNForget/actions?query=workflow%3ACI+is%3Asuccess++)
- click on the latest build
- download the artifact "RecNForget_TestBuild" that contains generated setup file
(downloading is currently only allowed for github users it seems)

# upcoming features in next release
- upgraded application to .net 5.0
- reworked most windows for a more consistent layout
- layout is STILL not final, but I don't expect any HUGE future changes except for Font/Sizes/Paddings/Margins :D - BUT
- RecNForget now has a dark mode, which is the new default
- window colors can now be changed using context menu -> Change theme -> (Select theme)
- selected file can now be exported as mp3, this action can be reached by
- pressing the 'X'-Button, while having the RecNForget-Window open or
- clicking the export button right next to the file name in the selected file control
- bitrate can be set using context menu -> open settings -> output -> Bitrate
- a dialog, prompting for an export name can be enabled using context menu -> open settings -> output -> 'prompt for exported file name'
- (otherwise filename is taken from original wav-file with mp3-extension)
- new recording timer control:
- users can now delay recording start/ stop with the new recording timer control
- this control can be toggled using context menu-> show recording timer control
- time spans used are entered right to left (seconds first) in the following format: d:hh:mm:ss
- if the "Stops after"- timer is enabled, a recording session  will be automatically ended after that time span
- "stop recording" can still be forced by hitting record again (if "Stops after"- timer is already running)
- "Stops after" can also be enabled while already recording
- if the "Starts after"- timer is enabled, the "start recording" action will be delayed by that time span
- "start recording" can be forced by triggering record again (if "Starts after"- timer is already running)
- switched to svg for icons
- popup info windows have been replaced with non blocking toast messages(errors and warnings), this also means we now have control over whether windows makes a sound on a toast message
(which RecNForget DOES NOT, so toast messages are actually viable now :D)
- selected file control now also shows file size and play length

# Features missing for version 1.0
- allow toggling input source (default output, or maybe mic/something else)?
- show record icon (red circle) in bottom right corner when recording (configurable)
- (meaning something always on top other than the main window showing recording details (recording time/buffersize etc)
- clean up source code for maintainability

# Copyright and License
RecNForget is written in C# using [.NET Core Runtime 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) and [WPF.](https://github.com/dotnet/wpf)

Code by DrCopyPaste [github.com/DrCopyPaste/RecNForget](https://github.com/DrCopyPaste/RecNForget)

Logo by DrCopyPaste (using icons from onlinewebfonts.com - see below)

Beep sounds by a cheap program from DrCopyPaste with love.

Click sounds by [lit-audio@gmx.de](mailto:lit-audio@gmx.de)/[soundcloud.com/wolfgankh](https://soundcloud.com/wolfgankh)



The following libaries are used by RecNForget:

[DotNetProjects.SVGImage v4.1.97](https://github.com/dotnetprojects/SVGImage) Copyright (c) DotNetProjects - [MIT](https://licenses.nuget.org/MIT)

[NAudio v2.0.1](https://github.com/naudio/NAudio) Copyright (c) Mark Heath & Contributors - [License](https://github.com/naudio/NAudio/blob/master/license.txt)

[Nerdbank.GitVersioning v3.4.231](https://github.com/aarnott/Nerdbank.GitVersioning) Copyright (c) Andrew Arnott - [MIT](https://licenses.nuget.org/MIT)

[Notifications.Wpf.Core v1.3.2](https://github.com/mjuen/Notifications.Wpf.Core) Copyright (c) Adrian Gaś, Simon Mauracher, Marcel Juen - [MIT](https://licenses.nuget.org/MIT)

[Octokit v0.50.0](https://github.com/octokit/octokit.net) Copyright (c) GitHub - [MIT](https://licenses.nuget.org/MIT)

[Ookii.Dialogs.Wpf v3.1.0](https://github.com/caioproiete/ookii-dialogs-wpf) Copyright (c) Ookii Dialogs Contributors - [BSD-3-Clause](https://licenses.nuget.org/BSD-3-Clause)

[System.Configuration.ConfigurationManager v5.0.0](https://github.com/dotnet/corefx) Copyright (c) Microsoft - [MIT](https://licenses.nuget.org/MIT)

[Unity v5.11.10](https://github.com/unitycontainer/unity) Copyright (c) Unity Container Project - [Apache 2.0](https://github.com/unitycontainer/unity/blob/v5.x/LICENSE)

[WiX Toolset build tools v3.11.2.4516](https://wixtoolset.org/) Copyright (c) .NET Foundation - [MS-RL](https://wixtoolset.org/about/license/)



The following images were used to create RecNForget:

accept_icon518267.svg was generated from [onlinewebfonts.com/icon/518267](https://www.onlinewebfonts.com/icon/518267)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

bugfix_icon140609.svg was generated from [onlinewebfonts.com/icon/140609](https://www.onlinewebfonts.com/icon/140609)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

cancel_icon428740.svg was generated from [onlinewebfonts.com/icon/428740](https://www.onlinewebfonts.com/icon/428740)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

close_icon349388.svg was generated from [onlinewebfonts.com/icon/349388](https://www.onlinewebfonts.com/icon/349388)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

download_icon602.svg was generated from [onlinewebfonts.com/icon/602](https://www.onlinewebfonts.com/icon/602)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

editSetting_icon451737.svg was generated from [onlinewebfonts.com/icon/451737](https://www.onlinewebfonts.com/icon/451737)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

export_icon343310.svg was generated from [onlinewebfonts.com/icon/343310](https://www.onlinewebfonts.com/icon/343310)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

folder_icon122592.svg was generated from [onlinewebfonts.com/icon/122592](https://www.onlinewebfonts.com/icon/122592)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

help_icon544157.svg was generated from [onlinewebfonts.com/icon/544157](https://www.onlinewebfonts.com/icon/544157)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

information_icon2132.svg was generated from [onlinewebfonts.com/icon/2132](https://www.onlinewebfonts.com/icon/2132)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

information_icon280732.svg was generated from [onlinewebfonts.com/icon/280732](https://www.onlinewebfonts.com/icon/280732)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

lightbulb_icon372420.svg was generated from [onlinewebfonts.com/icon/372420](https://www.onlinewebfonts.com/icon/372420)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

message_error_icon304479.svg was generated from [onlinewebfonts.com/icon/304479](https://www.onlinewebfonts.com/icon/304479)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

message_info_icon277439.svg was generated from [onlinewebfonts.com/icon/277439](https://www.onlinewebfonts.com/icon/277439)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

message_question_icon109120.svg was generated from [onlinewebfonts.com/icon/109120](https://www.onlinewebfonts.com/icon/109120)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

minimize_icon246566.svg was generated from [onlinewebfonts.com/icon/246566](https://www.onlinewebfonts.com/icon/246566)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

openFolder_icon356582.svg was generated from [onlinewebfonts.com/icon/356582](https://www.onlinewebfonts.com/icon/356582)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

pencil_icon79516.svg was generated from [onlinewebfonts.com/icon/79516](https://www.onlinewebfonts.com/icon/79516)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

plus_icon171498.svg was generated from [onlinewebfonts.com/icon/171498](https://www.onlinewebfonts.com/icon/171498)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

settings_icon40863.svg was generated from [onlinewebfonts.com/icon/40863](https://www.onlinewebfonts.com/icon/40863)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

shutDown_icon170336.svg was generated from [onlinewebfonts.com/icon/170336](https://www.onlinewebfonts.com/icon/170336)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

trash_icon77730.svg was generated from [onlinewebfonts.com/icon/77730](https://www.onlinewebfonts.com/icon/77730)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

windowOptions_icon510828.svg was generated from [onlinewebfonts.com/icon/510828](https://www.onlinewebfonts.com/icon/510828)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

windowSettings_icon248857.svg was generated from [onlinewebfonts.com/icon/248857](https://www.onlinewebfonts.com/icon/248857)

Icon made from [Icon Fonts](http://www.onlinewebfonts.com/icon) is licensed by CC BY 3.0

logo.svg was generated from [onlinewebfonts.com/icon/55290](https://www.onlinewebfonts.com/icon/55290) and [onlinewebfonts.com/icon/715](https://www.onlinewebfonts.com/icon/715)

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

