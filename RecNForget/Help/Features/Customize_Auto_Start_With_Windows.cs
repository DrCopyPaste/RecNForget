﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Auto_Start_With_Windows : HelpFeature
	{
		public Customize_Auto_Start_With_Windows()
		{
			MinVersion = HelpFeatureVersion.v0_3_release;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize auto start with windows";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can have RecNForget automatically start with Windows."
				},
				new HelpFeatureDetailLine()
				{
					Content = "(this is disabled by default)"
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
					Content = "- click on 'auto start'"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- exit the settings window by pressing the accept button"
				}
			};
		}
	}
}
