﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Click_Button_To_Play_Pause_Selected_File : HelpFeature
	{
		public Click_Button_To_Play_Pause_Selected_File()
		{
			MinVersion = HelpFeatureVersion.v0_2_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "click button to play/pause selected file";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can play the currently selected file by clicking the play-button in the main window."
				},
				new HelpFeatureDetailLine()
				{
					Content = "If that file is currently playing you can pause the replay by pressing the pause-button."
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "NOTE: Replaying or pausing a file will keep that file open, not allowing you to delete or rename it."
				}
			};
		}
	}
}
