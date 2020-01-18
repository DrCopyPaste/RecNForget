using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Click_Button_To_Open_Selected_File_In_Explorer : HelpFeature
	{
		public Click_Button_To_Open_Selected_File_In_Explorer()
		{
			MinVersion = HelpFeatureVersion.v0_2_preRelease;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "click button to open selected file in explorer";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can select the currently selected file in the windows explorer by clicking the folder-button at the bottom left of the main window."
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "NOTE: This will only open that folder in the explorer if no file is selected."
				}
			};
		}
	}
}
