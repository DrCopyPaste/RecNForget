using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Press_Hotkey_To_StopRecording_And_Save : HelpFeature
	{
		public Press_Hotkey_To_StopRecording_And_Save()
		{
			MinVersion = HelpFeatureVersion.v0_1_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "press hotkey to stop recording and save";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can stop recording audio and save as a new file by pressing the hotkey you assigned to 'Toggle Record'."
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "This hotkey is set to [Pause] by default."
				}
			};
		}
	}
}
