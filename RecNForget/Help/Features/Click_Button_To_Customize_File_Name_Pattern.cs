﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Click_Button_To_Customize_File_Name_Pattern : HelpFeature
	{
		public Click_Button_To_Customize_File_Name_Pattern()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "click button to customize file name pattern";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can customize the file pattern used to generate new file names by clicking on the pencil-button next to the recording path in the main window."
				}
			};
		}
	}
}