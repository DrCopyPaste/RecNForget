using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help.Features
{
	public class Select_Previous_File_In_Output_Folder : HelpFeature
	{
		public Select_Previous_File_In_Output_Folder()
		{
			MinVersion = HelpFeatureVersion.v0_4_WirsingKopf;
			FeatureClass = HelpFeatureClass.NewFeature;
			Title = "select previous file in output folder";
			HelpLines = new List<HelpFeatureDetailLine>()
			{
				new HelpFeatureDetailLine()
				{
					Content = "HelpText"
				}
			};
		}
	}
}
