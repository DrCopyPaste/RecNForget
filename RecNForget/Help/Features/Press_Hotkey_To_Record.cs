using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Press_Hotkey_To_Record : HelpFeature
	{
		public Press_Hotkey_To_Record()
		{
			MinVersion = HelpFeatureVersion.v0_1_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "press hotkey to record";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can start recording audio by pressing the hotkey you assigned to 'Toggle Record'."
				},
				new HelpFeatureDetailLine()
				{
					Content = "This hotkey is global, meaning it also works when the main window is not focused."
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
