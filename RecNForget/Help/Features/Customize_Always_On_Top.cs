using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Always_On_Top : HelpFeature
	{
		public Customize_Always_On_Top()
		{
			MinVersion = HelpFeatureVersion.v0_3_release;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize always on top";
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
