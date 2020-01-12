using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Press_Button_To_Record : HelpFeature
	{
		public Press_Button_To_Record()
		{
			MinVersion = HelpFeatureVersion.v0_1_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "press button to record";
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
