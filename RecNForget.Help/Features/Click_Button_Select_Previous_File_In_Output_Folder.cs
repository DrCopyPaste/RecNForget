﻿// <auto-generated />
using System.Collections.Generic;

namespace RecNForget.Help.Features
{
	public class Click_Button_Select_Previous_File_In_Output_Folder : HelpFeature
	{
		public Click_Button_Select_Previous_File_In_Output_Folder()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "click button to select previous file in output folder";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can select the previous file in the output folder by clicking the skip-left-button in the main window."
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "NOTE: Files in output folder are ordered by descending age, meaning 'previous file' will skip to the next file that is older than the current."
				}
			};
		}
	}
}
