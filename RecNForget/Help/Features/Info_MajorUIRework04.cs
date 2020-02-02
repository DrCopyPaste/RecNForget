using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Info_MajorUIRework04 : HelpFeature
	{
		public Info_MajorUIRework04()
		{
			Priority = 1000;
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.Information;
			Title = "major UI rework (v0.4)";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "consistently use white background for (almost) all controls"
				},
				new HelpFeatureDetailLine()
				{
					Content = "all action buttons now are in the same row at the bottom"
				},
				new HelpFeatureDetailLine()
				{
					Content = "added icons for most buttons"
				},
				new HelpFeatureDetailLine()
				{
					Content = "added skip buttons to navigate through output folder"
				},
				new HelpFeatureDetailLine()
				{
					Content = "clicking the open folder button now selects the selected file in explorer(or just opens folder if there is no result yet)"
				},
				new HelpFeatureDetailLine()
				{
					Content = "Added a stop - replay button(this also closes the file in RecNForget enabling you to move or rename it...)"
				},
				new HelpFeatureDetailLine()
				{
					Content = "window options and settings can be opened via context menu on the main window"
				},
				new HelpFeatureDetailLine()
				{
					Content = "added a help link to the menu"
				},
				new HelpFeatureDetailLine()
				{
					Content = "tray icon menu now shows the same menu as the windows options menu at the top left of the main window"
				}
			};
		}
	}
}
