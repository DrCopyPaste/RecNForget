using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Output_File_Pattern : HelpFeature
	{
		public Customize_Output_File_Pattern()
		{
			MinVersion = HelpFeatureVersion.v0_1_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize output file pattern";
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
