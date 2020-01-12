using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Auto_Start_With_Windows : HelpFeature
	{
		public Customize_Auto_Start_With_Windows()
		{
			MinVersion = HelpFeatureVersion.v0_3_release;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize auto start with windows";
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
