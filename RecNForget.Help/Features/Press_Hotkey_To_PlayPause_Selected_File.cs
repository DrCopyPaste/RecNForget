﻿// <auto-generated />
using System.Collections.Generic;

namespace RecNForget.Help.Features
{
	public class Press_Hotkey_To_PlayPause_Selected_File : HelpFeature
	{
		public Press_Hotkey_To_PlayPause_Selected_File()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "press hotkey to play or pause currently selected file";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can start playing or pausing the currently selected file by pressing the [Space] key."
				},
				new HelpFeatureDetailLine()
				{
					Content = "This hotkey only works when the main window is in focus."
				}
			};
		}
	}
}
