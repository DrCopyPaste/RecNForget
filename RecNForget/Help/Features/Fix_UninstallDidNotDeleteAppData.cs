﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Fix_UninstallDidNotDeleteAppData : HelpFeature
	{
		public Fix_UninstallDidNotDeleteAppData()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.BugFix;
			Title = "uninstalling will now also delete all application data";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "(under %APPDATA% machine configuration and in registry for autostart)"
				}
			};
		}
	}
}
