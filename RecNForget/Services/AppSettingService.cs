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
        private static string applicationName_Key = "RecNForget";
        private static string windowsAutoStartRegistryPath_Key = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private static string userConfigFileFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingService.applicationName_Key, "user.config");

        private static string checkForUpdateOnStart_Key = "CheckForUpdateOnStart";
        private static string autoSelectLastRecording_Key = "AutoSelectLastRecording";
        private static string autoReplayAudioAfterRecording_Key = "AutoReplayAudioAfterRecording";
        private static string playAudioFeedBackMarkingStartAndStopReplaying_Key = "PlayAudioFeedBackMarkingStartAndStopReplaying";
        private static string playAudioFeedBackMarkingStartAndStopRecording_Key = "PlayAudioFeedBackMarkingStartAndStopRecording";
        private static string minimizedToTray_Key = "MinimizedToTray";
        private static string hotKey_StartStopRecording_Key = "HotKey_StartStopRecording";
        private static string filenamePrefix_Key = "FilenamePrefix";
        private static string outputPath_Key = "OutputPath";
        private static string windowAlwaysOnTop_Key = "WindowAlwaysOnTop";
        private static string showBalloonTipsForRecording_Key = "ShowBalloonTipsForRecording";
        private static string showTipsAtApplicationStart_Key = "ShowTipsAtApplicationStart";
        private static string lastInstalledVersion_Key = "LastInstalledVersion";
        private static string mainWindowLeftX_Key = "MainWindowLeftX";
        private static string mainWindowTopY_Key = "MainWindowTopY";
        private static string outputPathControlVisible_Key = "OutputPathControlVisible";
        private static string selectedFileControlVisible_Key = "SelectedFileControlVisible";
        private static string windowTheme_Key = "WindowTheme";
        private static string uiScalingPercent_Key = "UiScalingPercent";

        public event PropertyChangedEventHandler PropertyChanged;

        public bool AutoStartWithWindows
        {
            get
            {
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(AppSettingService.windowsAutoStartRegistryPath_Key, true);
                var recNForgetAutoStartRegistry = regKey.GetValue(AppSettingService.applicationName_Key);

                if (recNForgetAutoStartRegistry != null)
                {
                    return true;
                }

                return false;
            }

            set
            {
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(AppSettingService.windowsAutoStartRegistryPath_Key, true);

                if (value == true)
                {
                    regKey.SetValue(AppSettingService.applicationName_Key, System.Reflection.Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    regKey.DeleteValue(AppSettingService.applicationName_Key);
                }

                OnPropertyChanged();
            }
        }

        public bool CheckForUpdateOnStart
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.checkForUpdateOnStart_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.checkForUpdateOnStart_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool AutoSelectLastRecording
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.autoSelectLastRecording_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.autoSelectLastRecording_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool AutoReplayAudioAfterRecording
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.autoReplayAudioAfterRecording_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.autoReplayAudioAfterRecording_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool PlayAudioFeedBackMarkingStartAndStopReplaying
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.playAudioFeedBackMarkingStartAndStopReplaying_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.playAudioFeedBackMarkingStartAndStopReplaying_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool PlayAudioFeedBackMarkingStartAndStopRecording
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.playAudioFeedBackMarkingStartAndStopRecording_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.playAudioFeedBackMarkingStartAndStopRecording_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool MinimizedToTray
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.minimizedToTray_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.minimizedToTray_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public string HotKey_StartStopRecording
        {
            get
            {
                return GetAppConfigSetting(AppSettingService.hotKey_StartStopRecording_Key);
            }

            set
            {
                SetAppConfigSetting(AppSettingService.hotKey_StartStopRecording_Key, value);
                OnPropertyChanged();
            }
        }

        public string FilenamePrefix
        {
            get
            {
                return GetAppConfigSetting(AppSettingService.filenamePrefix_Key);
            }

            set
            {
                SetAppConfigSetting(AppSettingService.filenamePrefix_Key, value);
                OnPropertyChanged();
            }
        }

        public string OutputPath
        {
            get
            {
                return GetAppConfigSetting(AppSettingService.outputPath_Key);
            }

            set
            {
                SetAppConfigSetting(AppSettingService.outputPath_Key, value);
                OnPropertyChanged();
            }
        }

        public bool WindowAlwaysOnTop
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.windowAlwaysOnTop_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.windowAlwaysOnTop_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool ShowBalloonTipsForRecording
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.showBalloonTipsForRecording_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.showBalloonTipsForRecording_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool ShowTipsAtApplicationStart
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.showTipsAtApplicationStart_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.showTipsAtApplicationStart_Key, value.ToString());
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
                    lastInstalledVersion = new Version(AppSettingService.GetAppConfigSetting(lastInstalledVersion_Key));
                }
                catch (Exception)
                {
                    // not installed before, or config was broken, default to 0.3.0.0, see above
                }

                return lastInstalledVersion;
            }

            set
            {
                SetAppConfigSetting(AppSettingService.lastInstalledVersion_Key, value.ToString());
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
                    retValue = double.Parse(GetAppConfigSetting(AppSettingService.mainWindowLeftX_Key));
                }
                catch (Exception)
                {
                    // do nothing, if there is an exception, this means the x/y settings just dont exist yet, window will be positioned due to Windows likings
                }

                return retValue;
            }

            set
            {
                SetAppConfigSetting(AppSettingService.mainWindowLeftX_Key, value.ToString());
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
                    retValue = double.Parse(GetAppConfigSetting(AppSettingService.mainWindowTopY_Key));
                }
                catch (Exception)
                {
                    // do nothing, if there is an exception, this means the x/y settings just dont exist yet, window will be positioned due to Windows likings
                }

                return retValue;
            }

            set
            {
                SetAppConfigSetting(AppSettingService.mainWindowTopY_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool OutputPathControlVisible
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.outputPathControlVisible_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.outputPathControlVisible_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public bool SelectedFileControlVisible
        {
            get
            {
                return Convert.ToBoolean(GetAppConfigSetting(AppSettingService.selectedFileControlVisible_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.selectedFileControlVisible_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public string WindowTheme
        {
            get
            {
                return GetAppConfigSetting(AppSettingService.windowTheme_Key);
            }

            set
            {
                SetAppConfigSetting(AppSettingService.selectedFileControlVisible_Key, value);
                OnPropertyChanged();
            }
        }

        public double UiScalingPercent
        {
            get
            {
                return double.Parse(GetAppConfigSetting(AppSettingService.uiScalingPercent_Key));
            }

            set
            {
                SetAppConfigSetting(AppSettingService.selectedFileControlVisible_Key, value.ToString());
                OnPropertyChanged();
            }
        }

        public static void RemoveAppConfigSettingFile()
        {
            // check if user config file exists
            FileInfo fileInfo = new FileInfo(userConfigFileFullPath);
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
                    File.Delete(userConfigFileFullPath);
                }

                // remove directory
                Directory.Delete(directoryInfo.FullName);
            }
        }

        public bool RestoreDefaultAppConfigSetting(string settingKey = null, bool overrideSetting = false)
        {
            bool configDidNotYetExist = false;

            // check if user config file exists
            FileInfo fileInfo = new FileInfo(userConfigFileFullPath);

            if (!fileInfo.Exists)
            {
                configDidNotYetExist = true;

                DirectoryInfo directoryInfo = new DirectoryInfo(fileInfo.DirectoryName);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }

                File.WriteAllText(userConfigFileFullPath, GetEmptyUserConfigFile());
            }

            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = userConfigFileFullPath }, ConfigurationUserLevel.None);

            Dictionary<string, string> defaultValues = new Dictionary<string, string>()
            {
                { AppSettingService.checkForUpdateOnStart_Key, "True" },
                { AppSettingService.autoSelectLastRecording_Key, "True" },
                { AppSettingService.autoReplayAudioAfterRecording_Key, "False" },
                { AppSettingService.playAudioFeedBackMarkingStartAndStopReplaying_Key, "False" },
                { AppSettingService.playAudioFeedBackMarkingStartAndStopRecording_Key, "True" },
                { AppSettingService.minimizedToTray_Key, "False" },
                { AppSettingService.hotKey_StartStopRecording_Key, "Key=Pause; Win=False; Alt=False; Ctrl=False; Shift=False" },
                { AppSettingService.filenamePrefix_Key, "RecNForget_" },
                { AppSettingService.outputPath_Key, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RecNForget") },
                { AppSettingService.windowAlwaysOnTop_Key, "False" },
                { AppSettingService.showBalloonTipsForRecording_Key, "True" },
                { AppSettingService.showTipsAtApplicationStart_Key, "True" },
                { AppSettingService.outputPathControlVisible_Key, "False" },
                { AppSettingService.selectedFileControlVisible_Key, "False" },
                { AppSettingService.windowTheme_Key, "Simple_White" },
                { AppSettingService.uiScalingPercent_Key, (100.0).ToString() },

                // dont show feature updates for versions below 0.3
                { AppSettingService.lastInstalledVersion_Key, "0.3.0.0" }
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

        private static string GetAppConfigSetting(string settingKey)
        {
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = userConfigFileFullPath }, ConfigurationUserLevel.None);

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
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = userConfigFileFullPath }, ConfigurationUserLevel.None);

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

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}