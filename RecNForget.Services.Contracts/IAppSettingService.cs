using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RecNForget.Services.Contracts
{
    public interface IAppSettingService : INotifyPropertyChanged
    {
        string RuntimeVersionString { get; }
        string RuntimeInformalVersionString { get; }

        bool AutoStartWithWindows { get; set; }

        bool CheckForUpdateOnStart { get; set; }

        bool AutoSelectLastRecording { get; set; }

        bool AutoReplayAudioAfterRecording { get; set; }

        bool PlayAudioFeedBackMarkingStartAndStopReplaying { get; set; }

        bool PlayAudioFeedBackMarkingStartAndStopRecording { get; set; }

        bool MinimizedToTray { get; set; }

        string HotKey_StartStopRecording { get; set; }

        string FilenamePrefix { get; set; }

        string OutputPath { get; set; }

        bool WindowAlwaysOnTop { get; set; }

        bool ShowBalloonTipsForRecording { get; set; }

        bool ShowTipsAtApplicationStart { get; set; }

        Version LastInstalledVersion { get; set; }

        double? MainWindowLeftX { get; set; }

        double? MainWindowTopY { get; set; }

        bool OutputPathControlVisible { get; set; }

        bool SelectedFileControlVisible { get; set; }

        string WindowTheme { get; set; }

        double UiScalingPercent { get; set; }
        int Mp3ExportBitrate { get; set; }
        bool PromptForExportFileName { get; set; }

        List<string> GetHotkeySettingAsList(string setting, string keyStart = "[", string keyEnd = "]");
        void RemoveAppConfigSettingFile();

        bool RestoreDefaultAppConfigSetting(string settingKey = null, bool overrideSetting = false);
    }
}
