﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Toast_Messages_For_Application_Actions : HelpFeature
	{
		public Customize_Toast_Messages_For_Application_Actions()
		{
			MinVersion = HelpFeatureVersion.v0_3_release;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize toast messages for application actions";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can configure RecNForget to show toast messages above the task bar for all the actions it does."
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
					Content = "- click on 'show toast messages'"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- exit the settings window by pressing the accept button"
				}
			};
		}
	}
}
