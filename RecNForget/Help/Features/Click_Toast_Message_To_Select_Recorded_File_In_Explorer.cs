using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Click_Toast_Message_To_Select_Recorded_File_In_Explorer : HelpFeature
	{
		public Click_Toast_Message_To_Select_Recorded_File_In_Explorer()
		{
			MinVersion = HelpFeatureVersion.v0_3_release;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "click toast message to select recorded file in explorer";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "You can click on the toast message shown when recording is finished to select that recorded file in the file explorer."
				},
				new HelpFeatureDetailLine()
				{
					Content = string.Empty
				},
				new HelpFeatureDetailLine()
				{
					Content = "NOTE: Toast messages will only show if that setting is active."
				}
			};
		}
	}
}
