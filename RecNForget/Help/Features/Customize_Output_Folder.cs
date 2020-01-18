﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Output_Folder : HelpFeature
	{
		public Customize_Output_Folder()
		{
			MinVersion = HelpFeatureVersion.v0_1_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize output folder";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can change the output folder RecNForget puts recorded files into."
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "To change this setting:"
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
					Content = "- click on 'File name pattern'"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- now enter the file pattern you desire"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- you can enter a placeholder such as (Date) to ensure a unique file name"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- if you don't, RecNForget will try to make file name unique on its own and just append a timestamp to you file name"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- enable you new file pattern by pressing [Return]"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- exit the settings window by pressing the accept button"
				}
			};
		}
	}
}
