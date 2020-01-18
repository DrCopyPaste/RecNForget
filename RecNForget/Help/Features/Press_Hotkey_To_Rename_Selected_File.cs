﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Press_Hotkey_To_Rename_Selected_File : HelpFeature
	{
		public Press_Hotkey_To_Rename_Selected_File()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "press hotkey to rename the currently selected file";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can rename the currently selected file by pressing the [Return] key."
				},
				new HelpFeatureDetailLine()
				{
					Content = "This hotkey only works when the main window is in focus."
				}
			};
		}
	}
}
