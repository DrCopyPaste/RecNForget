using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Auto_Select_Last_Recorded_File : HelpFeature
	{
		public Customize_Auto_Select_Last_Recorded_File()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize auto select last recorded file";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "HelpText"
				}
			};
		}
	}
}
