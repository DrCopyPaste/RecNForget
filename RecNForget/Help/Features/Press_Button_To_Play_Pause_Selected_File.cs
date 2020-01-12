using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Press_Button_To_Play_Pause_Selected_File : HelpFeature
	{
		public Press_Button_To_Play_Pause_Selected_File()
		{
			MinVersion = HelpFeatureVersion.v0_2_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "press button to play/pause selected file";
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
