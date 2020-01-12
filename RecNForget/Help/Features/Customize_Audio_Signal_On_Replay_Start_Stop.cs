using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Audio_Signal_On_Replay_Start_Stop : HelpFeature
	{
		public Customize_Audio_Signal_On_Replay_Start_Stop()
		{
			MinVersion = HelpFeatureVersion.v0_3_release;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize audio signal on replay start/stop";
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
