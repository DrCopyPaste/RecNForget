using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Customize_Toggle_Record_Hotkey : HelpFeature
	{
		public Customize_Toggle_Record_Hotkey()
		{
			MinVersion = HelpFeatureVersion.v0_1_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "customize 'toggle record' hotkey";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can change the global hotkey to start or stop recording."
				},
				new HelpFeatureDetailLine()
				{
					Content = "(By default this hotkey is set to [Pause])"
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "To change this setting:"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- open the menu at the top left of the main window"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- click on 'Open Settings'"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- click on 'Toggle Record'"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- close the new dialog by pressing the hotkey you want"
				},
				new HelpFeatureDetailLine()
				{
					Content = "- exit the settings window by pressing the accept button"
				}
			};
		}
	}
}
