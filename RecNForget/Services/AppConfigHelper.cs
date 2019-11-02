using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RecNForget.Services
{
	public class AppConfigHelper
	{
		public static void SetAppConfigSetting(string settingKey, string settingValue)
		{
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
			configuration.AppSettings.Settings[settingKey].Value = settingValue;
			configuration.Save();

			ConfigurationManager.RefreshSection("appSettings");
		}
	}
}