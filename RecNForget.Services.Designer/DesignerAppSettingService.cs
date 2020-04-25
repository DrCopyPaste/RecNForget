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
            get => false;

            set => throw new NotImplementedException();
        }

        public bool CheckForUpdateOnStart
        {
            get => false;

            set => throw new NotImplementedException();
        }

        public bool AutoSelectLastRecording
        {
            get => false;

            set => throw new NotImplementedException();
        }

        public bool AutoReplayAudioAfterRecording
        {
            get => false;

            set => throw new NotImplementedException();
        }

        public bool PlayAudioFeedBackMarkingStartAndStopReplaying
        {
            get => false;

            set => throw new NotImplementedException();
        }

        public bool PlayAudioFeedBackMarkingStartAndStopRecording
        {
            get => false;

            set => throw new NotImplementedException();
        }

        public bool MinimizedToTray
        {
            get => false;

            set => throw new NotImplementedException();
        }

        public string HotKey_StartStopRecording
        {
            get => "Key=Pause; Win=False; Alt=False; Ctrl=False; Shift=False";

            set => throw new NotImplementedException();
        }

        public string FilenamePrefix
        {
            get => "(Date)";

            set => throw new NotImplementedException();
        }

        public string OutputPath
        {
            get => @"C:\Some\Path";

            set => throw new NotImplementedException();
        }

        public bool WindowAlwaysOnTop
        {
            get => false;

            set => throw new NotImplementedException();
        }

        public bool ShowBalloonTipsForRecording
        {
            get => false;

            set => throw new NotImplementedException();
        }

        public bool ShowTipsAtApplicationStart
        {
            get => false;

            set => throw new NotImplementedException();
        }

        public Version LastInstalledVersion
        {
            get => new Version("42.42.42.42");

            set => throw new NotImplementedException();
        }

        public double? MainWindowLeftX
        {
            get => 100;

            set => throw new NotImplementedException();
        }

        public double? MainWindowTopY
        {
            get => 100;

            set => throw new NotImplementedException();
        }

        public bool OutputPathControlVisible
        {
            get => false;

            set => throw new NotImplementedException();
        }

        public bool SelectedFileControlVisible
        {
            get => false;

            set => throw new NotImplementedException();
        }

        public string WindowTheme
        {
            get => "Simple_White";

            set => throw new NotImplementedException();
        }

        public double UiScalingPercent
        {
            get => 100;

            set => throw new NotImplementedException();
        }

        public void RemoveAppConfigSettingFile() => throw new NotImplementedException();

        public bool RestoreDefaultAppConfigSetting(string settingKey = null, bool overrideSetting = false) => throw new NotImplementedException();
    }
}