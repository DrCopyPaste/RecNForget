﻿// <auto-generated />
using System.Collections.Generic;

namespace RecNForget.Help.Features
{
	public class Customize_Auto_Replay_After_Recording : HelpFeature
	{
		public Customize_Auto_Replay_After_Recording()
		{
			MinVersion = HelpFeatureVersion.v0_3_release;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize auto-replay after recording";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can have RecNForget automatically replay what you recorded immediately after recording it."
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
					Content = "- click on 'Open Settings' -> 'Application'"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- click on 'auto replay last recording'"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- exit the settings window by pressing the accept button"
				}
			};
		}
	}
}
