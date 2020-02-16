﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_SelectedFileControlVisibility : HelpFeature
	{
		public Customize_SelectedFileControlVisibility()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize selected file control visibility";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "The Selected File Control can be shown in the main window - this allows"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- viewing the currently selected file name"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- quick access to rename the selected file"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- quick access to delete the selected file"
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
					Content = "To enable or disable the Selected File Control:"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- open the menu at the top left of the main window"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- click on 'Selected File Control'"
				}
			};
		}
	}
}
