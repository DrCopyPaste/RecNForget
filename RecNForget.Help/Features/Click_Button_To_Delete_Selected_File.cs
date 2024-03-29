﻿// <auto-generated />
using System.Collections.Generic;

namespace RecNForget.Help.Features
{
	public class Click_Button_To_Delete_Selected_File : HelpFeature
	{
		public Click_Button_To_Delete_Selected_File()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "click button to delete selected file";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can delete the currently selected file by clicking on the trash-button next to the selected file in the main window."
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "NOTE: To do this you need to have the selected file control enabled (Context Menu-> 'Show Selected File Control')"
				}
			};
		}
	}
}
