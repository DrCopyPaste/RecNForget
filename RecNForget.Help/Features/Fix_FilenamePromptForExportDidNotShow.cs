﻿// <auto-generated />
using System.Collections.Generic;

namespace RecNForget.Help.Features
{
	public class Fix_FilenamePromptForExportDidNotShow : HelpFeature
	{
		public Fix_FilenamePromptForExportDidNotShow()
		{
			MinVersion = HelpFeatureVersion.v0_6_ChiliGarlicShrimps;
			FeatureClass = HelpFeatureClass.BugFix;
			Title = "Prompt for mp3 export file name now works properly.";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "Previously, checking this option would disable the whole export."
				}
			};
		}
	}
}
