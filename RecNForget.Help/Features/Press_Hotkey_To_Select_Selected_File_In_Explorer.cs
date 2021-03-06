﻿// <auto-generated />
using System.Collections.Generic;

namespace RecNForget.Help.Features
{
	public class Press_Hotkey_To_Select_Selected_File_In_Explorer : HelpFeature
	{
		public Press_Hotkey_To_Select_Selected_File_In_Explorer()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "press hotkey to select the currently selected file in explorer";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can select the currently selected file in the explorer by pressing the [Down-Arrow] key."
				},
				new HelpFeatureDetailLine()
				{
					Content = "This hotkey only works when the main window is in focus."
				}
			};
		}
	}
}
