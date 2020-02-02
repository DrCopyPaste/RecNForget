﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Fix_ClickingOpenfolderCrashes : HelpFeature
	{
		public Fix_ClickingOpenfolderCrashes()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.BugFix;
			Title = "clicking the open folder button does not crash RecNForget anymore";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "clicking the open folder button does not crash RecNForget anymore, if that folder does not (yet) exist"
				}
			};
		}
	}
}
