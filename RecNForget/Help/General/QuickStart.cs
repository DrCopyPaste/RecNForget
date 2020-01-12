using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.General
{
	public class QuickStart : HelpFeature
	{
		public QuickStart()
		{
			MinVersion = HelpFeatureVersion.v0_0_allVersions;
			FeatureClass = HelpFeatureClass.Information;
			Title = "Quick Start";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "The [Pause] key is the default hotkey to toggle recording:"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- just press once to start recording"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- press again to save the audio file"
				},
				new HelpFeatureDetailLine()
				{
					Content = " "
				},
				new HelpFeatureDetailLine()
				{
					Content = "Your recording is now saved to a default output folder."
				},
				new HelpFeatureDetailLine()
				{
					Content = "You can change both, hotkey and output folder in the settings menu."
				},
				new HelpFeatureDetailLine()
				{
					Content = "Click the button on the top left of the main menu to open settings."
				}
			};
		}
	}
}
