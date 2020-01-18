using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Click_Button_To_Delete_Selected_File : HelpFeature
	{
		public Click_Button_To_Delete_Selected_File()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "click button to delete selected file";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can delete the currently selected file by clicking on the trash-button next to the selected file in the main window."
				}
			};
		}
	}
}
