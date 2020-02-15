﻿using System;
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
        public static string ApplicationName = "RecNForget";
        public static string WindowsAutoStartRegistryPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

		public static string CheckForUpdateOnStart = "CheckForUpdateOnStart";
		public static string AutoSelectLastRecording = "AutoSelectLastRecording";
		public static string AutoReplayAudioAfterRecording = "AutoReplayAudioAfterRecording";
		public static string PlayAudioFeedBackMarkingStartAndStopReplaying = "PlayAudioFeedBackMarkingStartAndStopReplaying";
		public static string PlayAudioFeedBackMarkingStartAndStopRecording = "PlayAudioFeedBackMarkingStartAndStopRecording";
		public static string MinimizedToTray = "MinimizedToTray";
		public static string HotKey_StartStopRecording = "HotKey_StartStopRecording";
		public static string FilenamePrefix = "FilenamePrefix";
		public static string OutputPath = "OutputPath";
		public static string WindowAlwaysOnTop = "WindowAlwaysOnTop";
		public static string ShowBalloonTipsForRecording = "ShowBalloonTipsForRecording";
        public static string ShowTipsAtApplicationStart = "ShowTipsAtApplicationStart";
        public static string LastInstalledVersion = "LastInstalledVersion";

        public static string MainWindowLeftX = "MainWindowLeftX";
        public static string MainWindowTopY = "MainWindowTopY";

        private static string UserConfigFileFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingHelper.ApplicationName, "user.config");

        public static string GetEmptyUserConfigFile()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
 <configuration>
  <startup>
   <supportedRuntime version = ""v4.0"" sku = "".NETFramework,Version=v4.8"" />
  </startup>
  <appSettings>
  </appSettings>
 </configuration>";
        }

        public static Version GetLastInstalledVersion()
        {
            var lastInstalledVersion = new Version("0.3.0.0");

            try
            {
                lastInstalledVersion = new Version(AppSettingHelper.GetAppConfigSetting(LastInstalledVersion));
            }
            catch(Exception)
            {
                // not installed before, or config was broken, default to 0.3.0.0, see above
            }

            return lastInstalledVersion;
        }

        public static void UpdateLastInstalledVersion(Version version)
        {
            AppSettingHelper.SetAppConfigSetting(LastInstalledVersion, version.ToString());
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

        public static void RemoveAppConfigSettingFile()
        {
            // check if user config file exists
            FileInfo fileInfo = new FileInfo(UserConfigFileFullPath);
            DirectoryInfo directoryInfo = new DirectoryInfo(fileInfo.DirectoryName);

            if (!directoryInfo.Exists)
            {
                // we are good, nothing to remove here...
            }
            else
            {
                if (fileInfo.Exists)
                {
                    // remove file if it exists
                    File.Delete(UserConfigFileFullPath);
                }

                // remove directory
                Directory.Delete(directoryInfo.FullName);
            }
        }


        public static bool RestoreDefaultAppConfigSetting(string settingKey = null, bool overrideSetting = false)
		{
            bool configDidNotYetExist = false;

            // check if user config file exists
            FileInfo fileInfo = new FileInfo(UserConfigFileFullPath);

            if (!fileInfo.Exists)
            {
                configDidNotYetExist = true;

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
				{ AppSettingHelper.CheckForUpdateOnStart, "True" },
				{ AppSettingHelper.AutoSelectLastRecording, "True" },
				{ AppSettingHelper.AutoReplayAudioAfterRecording, "False" },
				{ AppSettingHelper.PlayAudioFeedBackMarkingStartAndStopReplaying, "False" },
				{ AppSettingHelper.PlayAudioFeedBackMarkingStartAndStopRecording, "True" },
				{ AppSettingHelper.MinimizedToTray, "False" },
				{ AppSettingHelper.HotKey_StartStopRecording, "Key=Pause; Win=False; Alt=False; Ctrl=False; Shift=False" },
				{ AppSettingHelper.FilenamePrefix, "RecNForget_" },
				{ AppSettingHelper.OutputPath, @"C:\tmp" },
				{ AppSettingHelper.WindowAlwaysOnTop, "False" },
                { AppSettingHelper.ShowBalloonTipsForRecording, "True" },
                { AppSettingHelper.ShowTipsAtApplicationStart, "True" },

                // dont show feature updates for versions below 0.3
                { AppSettingHelper.LastInstalledVersion, "0.3.0.0" }
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

            return configDidNotYetExist;
        }
	}
}