﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Audio_Signal_On_Record_Start_Stop : HelpFeature
	{
		public Customize_Audio_Signal_On_Record_Start_Stop()
		{
			MinVersion = HelpFeatureVersion.v0_3_release;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize audio signal on record start/stop";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can have RecNForget signal the start and end of its recording with a short audio feedback."
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