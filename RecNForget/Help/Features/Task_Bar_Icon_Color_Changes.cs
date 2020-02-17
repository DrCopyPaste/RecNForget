using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Task_Bar_Icon_Color_Changes : HelpFeature
	{
		public Task_Bar_Icon_Color_Changes()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "task bar icon color changes";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "RecNForget changes the background color of the task bar icon to signal its current state:"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- transparent: ready to record/play (no files are held open)"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- red: currently recording"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- green: currently playing (or paused) audio (selected file is held open)"
				}
			};
		}
	}
}
