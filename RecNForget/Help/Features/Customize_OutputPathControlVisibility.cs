﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_OutputPathControlVisibility : HelpFeature
	{
		public Customize_OutputPathControlVisibility()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize output path control visibility";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "The Output Path Control can be shown in the main window - this allows"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- quick access to the output path setting"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- quick access to the file name prefix setting"
				},
				new HelpFeatureDetailLine()
				{
					Content = "(this is control is disabled by default)"
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "To enable or disable the Output Path Control:"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- open the menu at the top left of the main window"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- click on 'Output Path Control'"
				}
			};
		}
	}
}
