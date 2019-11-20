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
        public static string AutoReplayAudioAfterRecording = "AutoReplayAudioAfterRecording";
		public static string PlayAudioFeedBackMarkingStartAndStopReplaying = "PlayAudioFeedBackMarkingStartAndStopReplaying";
		public static string PlayAudioFeedBackMarkingStartAndStopRecording = "PlayAudioFeedBackMarkingStartAndStopRecording";
		public static string MinimizedToTray = "MinimizedToTray";
		public static string HotKey_StartStopRecording = "HotKey_StartStopRecording";
		public static string FilenamePrefix = "FilenamePrefix";
		public static string OutputPath = "OutputPath";
		public static string WindowAlwaysOnTop = "WindowAlwaysOnTop";
		public static string ShowBalloonTipsForRecording = "ShowBalloonTipsForRecording";

        private static string UserConfigFileFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RecNForget", "user.config");

        public static string GetEmptyUserConfigFile()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
 <configuration>
  <startup>
   <supportedRuntime version = ""v4.0"" sku = "".NETFramework,Version=v4.7"" />
  </startup>
  <appSettings>
  </appSettings>
 </configuration>";
        }

        public static string GetAppConfigSetting(string settingKey)
        {
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = UserConfigFileFullPath }, ConfigurationUserLevel.None);

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
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = UserConfigFileFullPath }, ConfigurationUserLevel.None);

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
            // check if user config file exists
            FileInfo fileInfo = new FileInfo(UserConfigFileFullPath);

            if (!fileInfo.Exists)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(fileInfo.DirectoryName);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }

                File.WriteAllText(UserConfigFileFullPath, GetEmptyUserConfigFile());
            }

            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = UserConfigFileFullPath }, ConfigurationUserLevel.None);

            Dictionary<string, string> defaultValues = new Dictionary<string, string>()
			{
				{ AppSettingHelper.AutoReplayAudioAfterRecording, "False" },
				{ AppSettingHelper.PlayAudioFeedBackMarkingStartAndStopReplaying, "False" },
				{ AppSettingHelper.PlayAudioFeedBackMarkingStartAndStopRecording, "True" },
				{ AppSettingHelper.MinimizedToTray, "False" },
				{ AppSettingHelper.HotKey_StartStopRecording, "Key=F12; Win=False; Alt=False; Ctrl=True; Shift=False" },
				{ AppSettingHelper.FilenamePrefix, "RecNForget_" },
				{ AppSettingHelper.OutputPath, @"C:\tmp" },
				{ AppSettingHelper.WindowAlwaysOnTop, "False" },
				{ AppSettingHelper.ShowBalloonTipsForRecording, "True" }
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