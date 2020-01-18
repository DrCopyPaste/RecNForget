using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Click_Button_To_Stop_Recording_And_Save : HelpFeature
	{
		public Click_Button_To_Stop_Recording_And_Save()
		{
			MinVersion = HelpFeatureVersion.v0_1_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "click button to stop recording and save";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can stop recording audio and save as a new file by clicking the record button at the bottom right of the main window."
				}
			};
		}
	}
}
