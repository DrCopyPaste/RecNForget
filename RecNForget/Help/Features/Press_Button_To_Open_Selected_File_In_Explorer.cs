using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Press_Button_To_Open_Selected_File_In_Explorer : HelpFeature
	{
		public Press_Button_To_Open_Selected_File_In_Explorer()
		{
			MinVersion = HelpFeatureVersion.v0_2_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "press button to open selected file in explorer";
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
