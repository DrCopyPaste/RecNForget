using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Toast_Messages_For_Application_Actions : HelpFeature
	{
		public Customize_Toast_Messages_For_Application_Actions()
		{
			MinVersion = HelpFeatureVersion.v0_3_release;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize toast messages for application actions";
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
