using RecNForget.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecNForget.Services;

public class UserConfigurationService : IAppSettingService
{
    private static readonly string configurationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(RecNForget), "user.config.json");
    private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

    public UserConfigurationService() : this(firstStart: false)
    { }

    public UserConfigurationService(bool firstStart = true)
    {
        FirstApplicationStart = firstStart;
    }

    public static UserConfigurationService Init()
    {
        var fileInfo = new FileInfo(configurationFilePath);
        if (!fileInfo.Exists)
        {
            fileInfo.Directory.Create();
            var newFile = fileInfo.CreateText();
            newFile.Close();
        }

        using var sr = new StreamReader(fileInfo.FullName);
        var fileContents = sr.ReadToEnd();

        if (fileContents == string.Empty) return new UserConfigurationService(firstStart: true);
        return JsonSerializer.Deserialize<UserConfigurationService>(fileContents);
    }

    public void Persist()
    {
        using var sw = new StreamWriter(configurationFilePath);
        var json = JsonSerializer.Serialize(this, jsonSerializerOptions);
        sw.Write(json);
    }

    public async Task PersistAsync()
    {
        using var sw = new StreamWriter(configurationFilePath);
        var json = JsonSerializer.Serialize(this, jsonSerializerOptions);
        await sw.WriteAsync(json);
    }

    #region actual user data
    public bool AutoStartWithWindows { get; set; } = false;
    public bool CheckForUpdateOnStart { get; set; } = true;
    public bool AutoSelectLastRecording { get; set; } = true;
    public bool AutoReplayAudioAfterRecording { get; set; } = false;
    public bool PlayAudioFeedBackMarkingStartAndStopReplaying { get; set; } = false;
    public bool PlayAudioFeedBackMarkingStartAndStopRecording { get; set; } = true;
    public bool MinimizedToTray { get; set; } = false;
    public string HotKey_StartStopRecording { get; set; } = "Key=Pause; Win=False; Alt=False; Ctrl=False; Shift=False";
    public string FilenamePrefix { get; set; } = string.Empty;
    public string OutputPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), nameof(RecNForget));
    public bool WindowAlwaysOnTop { get; set; } = false;
    public bool ShowBalloonTipsForRecording { get; set; } = true;
    public bool ShowTipsAtApplicationStart { get; set; } = true;
    public Version LastInstalledVersion { get; set; } = new Version(0, 7, 0, 0);
    public double? MainWindowLeftX { get; set; } = 10;
    public double? MainWindowTopY { get; set; } = 10;
    public bool OutputPathControlVisible { get; set; } = false;
    public bool SelectedFileControlVisible { get; set; } = false;
    public string WindowTheme { get; set; } = "simple_white";
    public double UiScalingPercent { get; set; } = 100;
    public int Mp3ExportBitrate { get; set; } = 320;
    public bool PromptForExportFileName { get; set; } = false;
    public bool RecordingTimerStopAfterIsEnabled { get; set; } = false;
    public bool RecordingTimerStartAfterIsEnabled { get; set; } = false;
    public bool RecordingTimerControlVisible { get; set; } = false;
    public string RecordingTimerStartAfterMax { get; set; } = "0:00:00:00";
    public string RecordingTimerStopAfterMax { get; set; } = "0:00:00:00";
    public string ExportOutputPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), nameof(RecNForget), "Export");

    #endregion

    [JsonIgnore]
    public bool FirstApplicationStart { get; private set; } = false;

    [JsonIgnore]
    public string RuntimeVersionString => "0.3.0";

    [JsonIgnore]
    public string RuntimeInformalVersionString => "0.3.0+abcdef123";
    public List<string> GetHotkeySettingAsList(string setting, string keyStart = "[", string keyEnd = "]")
    {
        List<string> keys = new List<string>();

        // modifier keys
        if (setting.Contains("Shift=True"))
        {
            keys.Add(keyStart + "Shift" + keyEnd);
        }

        if (setting.Contains("Ctrl=True"))
        {
            keys.Add(keyStart + "Ctrl" + keyEnd);
        }

        if (setting.Contains("Alt=True"))
        {
            keys.Add(keyStart + "Alt" + keyEnd);
        }

        if (setting.Contains("Win=True"))
        {
            keys.Add(keyStart + "Win" + keyEnd);
        }

        var actualKey = setting.Split(';')[0].Replace("Key=", string.Empty);
        if (actualKey != string.Empty && actualKey != "None")
        {
            keys.Add(keyStart + actualKey + keyEnd);
        }

        return keys;
    }

    public void RemoveAppConfigSettingFile()
    {
        
    }

    public bool RestoreDefaultAppConfigSetting(string settingKey = null, bool overrideSetting = false)
    {
        return true;
    }

    public bool UpdateConfigVersion()
    {
        var previouslyInstalledVersion = LastInstalledVersion;
        var currentFileVersion = new Version(ThisAssembly.AssemblyFileVersion);

        LastInstalledVersion = currentFileVersion;

        return (currentFileVersion.CompareTo(previouslyInstalledVersion) > 0);
    }
}
