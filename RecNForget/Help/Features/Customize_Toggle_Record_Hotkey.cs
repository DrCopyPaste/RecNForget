using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Toggle_Record_Hotkey : HelpFeature
	{
		public Customize_Toggle_Record_Hotkey()
		{
			MinVersion = HelpFeatureVersion.v0_1_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize 'toggle record' hotkey";
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
