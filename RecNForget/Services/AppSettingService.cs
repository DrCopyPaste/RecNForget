using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RecNForget.Services
{
    public class AppSettingService : INotifyPropertyChanged
    {
        private static string ApplicationName_Key = "RecNForget";
        private static string WindowsAutoStartRegistryPath_Key = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private static string UserConfigFileFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingService.ApplicationName_Key, "user.config");

        private static string CheckForUpdateOnStart_Key = "CheckForUpdateOnStart";
        private static string AutoSelectLastRecording_Key = "AutoSelectLastRecording";
        private static string AutoReplayAudioAfterRecording_Key = "AutoReplayAudioAfterRecording";
        private static string PlayAudioFeedBackMarkingStartAndStopReplaying_Key = "PlayAudioFeedBackMarkingStartAndStopReplaying";
        private static string PlayAudioFeedBackMarkingStartAndStopRecording_Key = "PlayAudioFeedBackMarkingStartAndStopRecording";
        private static string MinimizedToTray_Key = "MinimizedToTray";
        private static string HotKey_StartStopRecording_Key = "HotKey_StartStopRecording";
        private static string FilenamePrefix_Key = "FilenamePrefix";
        private static string OutputPath_Key = "OutputPath";
        private static string WindowAlwaysOnTop_Key = "WindowAlwaysOnTop";
        private static string ShowBalloonTipsForRecording_Key = "ShowBalloonTipsForRecording";
        private static string ShowTipsAtApplicationStart_Key = "ShowTipsAtApplicationStart";
        private static string LastInstalledVersion_Key = "LastInstalledVersion";
        private static string MainWindowLeftX_Key = "MainWindowLeftX";
        private static string MainWindowTopY_Key = "MainWindowTopY";
        private static string OutputPathControlVisible_Key = "OutputPathControlVisible";
        private static string SelectedFileControlVisible_Key = "SelectedFileControlVisible";

        private static string GetEmptyUserConfigFile()
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

        public bool AutoStartWithWindows
        {
            get
            {
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(AppSettingService.WindowsAutoStartRegistryPath_Key, true);
                var recNForgetAutoStartRegistry = regKey.GetValue(AppSettingService.ApplicationName_Key);

                if (recNForgetAutoStartRegistry != null)
                {
                    return true;
                }

                return false;
            }
            set
            {
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(AppSettingService.WindowsAutoStartRegistryPath_Key, true);

                if (value == true)
                {
                    regKey.SetValue(AppSettingService.ApplicationName_Key, System.Reflection.Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    regKey.DeleteValue(AppSettingService.ApplicationName_Key);
                }

                OnPropertyChanged();
            }
        }

        public bool CheckForUpdateOnStart
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.CheckForUpdateOnStart_Key));
            }
            set
            {
                SetAppConfigSetting(AppSettingService.CheckForUpdateOnStart_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool AutoSelectLastRecording
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.AutoSelectLastRecording_Key));
            }
            set
            {
                SetAppConfigSetting(AppSettingService.AutoSelectLastRecording_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool AutoReplayAudioAfterRecording
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.AutoReplayAudioAfterRecording_Key));
            }
            set
            {
                SetAppConfigSetting(AppSettingService.AutoReplayAudioAfterRecording_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool PlayAudioFeedBackMarkingStartAndStopReplaying
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying_Key));
            }
            set
            {
                SetAppConfigSetting(AppSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool PlayAudioFeedBackMarkingStartAndStopRecording
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.PlayAudioFeedBackMarkingStartAndStopRecording_Key));
            }
            set
            {
                SetAppConfigSetting(AppSettingService.PlayAudioFeedBackMarkingStartAndStopRecording_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool MinimizedToTray
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.MinimizedToTray_Key));
            }
            set
            {
                SetAppConfigSetting(AppSettingService.MinimizedToTray_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public string HotKey_StartStopRecording
        {
            get
            {
                return GetAppConfigSetting(AppSettingService.HotKey_StartStopRecording_Key);
            }
            set
            {
                SetAppConfigSetting(AppSettingService.HotKey_StartStopRecording_Key, value);
                OnPropertyChanged();
            }
        }

        public string FilenamePrefix
        {
            get
            {
                return GetAppConfigSetting(AppSettingService.FilenamePrefix_Key);
            }
            set
            {
                SetAppConfigSetting(AppSettingService.FilenamePrefix_Key, value);
                OnPropertyChanged();
            }
        }

        public string OutputPath
        {
            get
            {
                return GetAppConfigSetting(AppSettingService.OutputPath_Key);
            }
            set
            {
                SetAppConfigSetting(AppSettingService.OutputPath_Key, value);
                OnPropertyChanged();
            }
        }

        public bool WindowAlwaysOnTop
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.WindowAlwaysOnTop_Key));
            }
            set
            {
                SetAppConfigSetting(AppSettingService.WindowAlwaysOnTop_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool ShowBalloonTipsForRecording
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.ShowBalloonTipsForRecording_Key));
            }
            set
            {
                SetAppConfigSetting(AppSettingService.ShowBalloonTipsForRecording_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool ShowTipsAtApplicationStart
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.ShowTipsAtApplicationStart_Key));
            }
            set
            {
                SetAppConfigSetting(AppSettingService.ShowTipsAtApplicationStart_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public Version LastInstalledVersion
        {
            get
            {
                var lastInstalledVersion = new Version("0.3.0.0");

                try
                {
                    lastInstalledVersion = new Version(AppSettingService.GetAppConfigSetting(LastInstalledVersion_Key));
                }
                catch (Exception)
                {
                    // not installed before, or config was broken, default to 0.3.0.0, see above
                }

                return lastInstalledVersion;
            }
            set
            {
                SetAppConfigSetting(AppSettingService.LastInstalledVersion_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public double? MainWindowLeftX
        {
            get
            {
                double? retValue = null;

                // try read last window positon
                try
                {
                    retValue = double.Parse(GetAppConfigSetting(AppSettingService.MainWindowLeftX_Key));
                }
                catch (Exception)
                {
                    // do nothing, if there is an exception, this means the x/y settings just dont exist yet, window will be positioned due to Windows likings
                }

                return retValue;
            }
            set
            {
                SetAppConfigSetting(AppSettingService.MainWindowLeftX_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public double? MainWindowTopY
        {
            get
            {
                double? retValue = null;

                // try read last window positon
                try
                {
                    retValue = double.Parse(GetAppConfigSetting(AppSettingService.MainWindowTopY_Key));
                }
                catch (Exception)
                {
                    // do nothing, if there is an exception, this means the x/y settings just dont exist yet, window will be positioned due to Windows likings
                }
                
                return retValue;
            }
            set
            {
                SetAppConfigSetting(AppSettingService.MainWindowTopY_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool OutputPathControlVisible
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.OutputPathControlVisible_Key));
            }
            set
            {
                SetAppConfigSetting(AppSettingService.OutputPathControlVisible_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool SelectedFileControlVisible
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.SelectedFileControlVisible_Key));
            }
            set
            {
                SetAppConfigSetting(AppSettingService.SelectedFileControlVisible_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static string GetAppConfigSetting(string settingKey)
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

		private static void SetAppConfigSetting(string settingKey, string settingValue)
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

        public bool RestoreDefaultAppConfigSetting(string settingKey = null, bool overrideSetting = false)
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
				{ AppSettingService.CheckForUpdateOnStart_Key, "True" },
				{ AppSettingService.AutoSelectLastRecording_Key, "True" },
				{ AppSettingService.AutoReplayAudioAfterRecording_Key, "False" },
				{ AppSettingService.PlayAudioFeedBackMarkingStartAndStopReplaying_Key, "False" },
				{ AppSettingService.PlayAudioFeedBackMarkingStartAndStopRecording_Key, "True" },
				{ AppSettingService.MinimizedToTray_Key, "False" },
				{ AppSettingService.HotKey_StartStopRecording_Key, "Key=Pause; Win=False; Alt=False; Ctrl=False; Shift=False" },
				{ AppSettingService.FilenamePrefix_Key, "RecNForget_" },
				{ AppSettingService.OutputPath_Key, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RecNForget") },
				{ AppSettingService.WindowAlwaysOnTop_Key, "False" },
                { AppSettingService.ShowBalloonTipsForRecording_Key, "True" },
                { AppSettingService.ShowTipsAtApplicationStart_Key, "True" },
                { AppSettingService.OutputPathControlVisible_Key, "False" },
                { AppSettingService.SelectedFileControlVisible_Key, "False" },

                // dont show feature updates for versions below 0.3
                { AppSettingService.LastInstalledVersion_Key, "0.3.0.0" }
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