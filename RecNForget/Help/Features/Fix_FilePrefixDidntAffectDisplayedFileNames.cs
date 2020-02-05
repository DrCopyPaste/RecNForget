﻿using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Fix_FilePrefixDidntAffectDisplayedFileNames : HelpFeature
	{
		public Fix_FilePrefixDidntAffectDisplayedFileNames()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.BugFix;
			Title = "file prefix changes affect displayed file name";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "changing the file prefix now also changes the displayed file name with output path"
				}
			};
		}
	}
}