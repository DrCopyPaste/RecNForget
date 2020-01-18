using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Click_Button_To_Select_Next_File_In_Output_Folder : HelpFeature
	{
		public Click_Button_To_Select_Next_File_In_Output_Folder()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "click button to select next file in output folder";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can select the next file in the output folder by clicking the skip-right-button in the main window."
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "NOTE: Files in output folder are ordered by descending age, meaning 'next file' will skip to the next file that is newer than the current."
				}
			};
		}
	}
}
