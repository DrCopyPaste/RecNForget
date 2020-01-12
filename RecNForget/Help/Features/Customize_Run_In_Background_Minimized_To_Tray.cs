using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Run_In_Background_Minimized_To_Tray : HelpFeature
	{
		public Customize_Run_In_Background_Minimized_To_Tray()
		{
			MinVersion = HelpFeatureVersion.v0_3_release;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize run in background (minimized to tray)";
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
