﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Click_Button_To_Stop_Playing_Selected_File : HelpFeature
	{
		public Click_Button_To_Stop_Playing_Selected_File()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "click button to stop playing selected file";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can stop playing the currently selected file stop button in the main window."
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "NOTE: Stopping replaying will close the file, enabling you to rename or delete it."
				}
			};
		}
	}
}
