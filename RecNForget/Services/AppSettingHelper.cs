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
        public static string GetAppConfigSetting(string settingKey)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            if (configuration.AppSettings.Settings.AllKeys.Contains(settingKey))
            {
                return configuration.AppSettings.Settings[settingKey].Value;
            }
            else
            {
                throw new ArgumentNullException("User configuration seems corrupted, please go to programs/features menu and hit 'Repair' on RecNForget.");
            }
        }

		public static void SetAppConfigSetting(string settingKey, string settingValue)
		{
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

			if (configuration.AppSettings.Settings.AllKeys.Contains(settingKey))
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
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

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
					if (!configuration.AppSettings.Settings.AllKeys.Contains(setting.Key) || overrideSetting)
					{
						SetAppConfigSetting(setting.Key, setting.Value);
					}
				}
			}
			else if (defaultValues.ContainsKey(settingKey))
			{
				if (!configuration.AppSettings.Settings.AllKeys.Contains(settingKey) || overrideSetting)
				{
					SetAppConfigSetting(settingKey, defaultValues[settingKey]);
				}
			}
		}
	}
}