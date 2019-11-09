using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RecNForget.Services
{
	public class AppSettingHelper
	{
		public static void SetAppConfigSetting(string settingKey, string settingValue)
		{
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

			if (ConfigurationManager.AppSettings.AllKeys.Contains(settingKey))
			{
				configuration.AppSettings.Settings[settingKey].Value = settingValue;
			}
			else
			{
				configuration.AppSettings.Settings.Add(settingKey, settingValue);
			}

			configuration.Save();
			ConfigurationManager.RefreshSection("appSettings");
		}

		public static void RestoreDefaultAppConfigSetting(string settingKey = null, bool overrideSetting = false)
		{
			Dictionary<string, string> defaultValues = new Dictionary<string, string>()
			{
				{ "AutoReplayAudioAfterRecording", "False" },
				{ "PlayAudioFeedBackMarkingStartAndStopReplaying", "False" },
				{ "PlayAudioFeedBackMarkingStartAndStopRecording", "True" },
				{ "MinimizedToTray", "False" },
				{ "HotKey_StartStopRecording", "Key=F12; Win=False; Alt=False; Ctrl=True; Shift=False" },
				{ "FilenamePrefix", "RecNForget_" },
				{ "OutputPath", @"C:\tmp" },
				{ "WindowAlwaysOnTop", "False" },
				{ "ShowBalloonTipsForRecording", "True" }
			};

			if (settingKey == null)
			{
				foreach (var setting in defaultValues)
				{
					if (!ConfigurationManager.AppSettings.AllKeys.Contains(setting.Key) || overrideSetting)
					{
						SetAppConfigSetting(setting.Key, setting.Value);
					}
				}
			}
			else if (defaultValues.ContainsKey(settingKey))
			{
				if (!ConfigurationManager.AppSettings.AllKeys.Contains(settingKey) || overrideSetting)
				{
					SetAppConfigSetting(settingKey, defaultValues[settingKey]);
				}
			}
		}
	}
}