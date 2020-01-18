﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Auto_Select_Last_Recorded_File : HelpFeature
	{
		public Customize_Auto_Select_Last_Recorded_File()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize auto select last recorded file";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can have RecNForget automatically select a file after recording it (changing the selected file control you can navigate via the skip buttons)"
				},
				new HelpFeatureDetailLine()
				{
					Content = "(this is enabled by default)"
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "To enable or disable this setting:"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- open the menu at the top left of the main window"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- click on 'Open Settings'"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- click on 'sound on toggle record'"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- exit the settings window by pressing the accept button"
				}
			};
		}
	}
}
