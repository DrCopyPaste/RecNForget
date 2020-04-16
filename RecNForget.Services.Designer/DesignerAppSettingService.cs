using System;
using System.ComponentModel;
using RecNForget.Services.Contracts;

namespace RecNForget.Services.Designer
{
    public class DesignerAppSettingService : IAppSettingService
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool AutoStartWithWindows
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool CheckForUpdateOnStart
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool AutoSelectLastRecording
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool AutoReplayAudioAfterRecording
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool PlayAudioFeedBackMarkingStartAndStopReplaying
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool PlayAudioFeedBackMarkingStartAndStopRecording
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool MinimizedToTray
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string HotKey_StartStopRecording
        {
            get
            {
                return "Key=Pause; Win=False; Alt=False; Ctrl=False; Shift=False";
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string FilenamePrefix
        {
            get
            {
                return "(Date)";
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string OutputPath
        {
            get
            {
                return @"C:\Some\Path";
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool WindowAlwaysOnTop
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ShowBalloonTipsForRecording
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ShowTipsAtApplicationStart
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Version LastInstalledVersion
        {
            get
            {
                return new Version("42.42.42.42");
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public double? MainWindowLeftX
        {
            get
            {
                return 100;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public double? MainWindowTopY
        {
            get
            {
                return 100;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool OutputPathControlVisible
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool SelectedFileControlVisible
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string WindowTheme
        {
            get
            {
                return "Simple_White";
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public double UiScalingPercent
        {
            get
            {
                return 100;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void RemoveAppConfigSettingFile()
        {
            throw new NotImplementedException();
        }

        public bool RestoreDefaultAppConfigSetting(string settingKey = null, bool overrideSetting = false)
        {
            throw new NotImplementedException();
        }
    }
}