﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Press_Hotkey_To_Select_Previous_File_In_Output_Folder : HelpFeature
	{
		public Press_Hotkey_To_Select_Previous_File_In_Output_Folder()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "press hotkey to skip to previous file in the output folder";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can select the previous file in the output folder by pressing the [Left-Arrow] key."
				},
				new HelpFeatureDetailLine()
				{
					Content = "This hotkey only works when the main window is in focus."
				}
			};
		}
	}
}
