using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace CustomBackgroundAddon.Settings;

public partial class SettingsManager : ObservableObject
{
    [ObservableProperty] private bool _keepEditorBackground = false;
    [ObservableProperty] private string _currentBackgroundPath = "";
    [ObservableProperty] private double _backgroundOpacity = 5.0;
    [ObservableProperty] private double _backgroundBlur = 0.0;

    public static string AppDataFolderPath { get; } =
        System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SkEditor", "Addons",
                     CustomBackgroundAddon.Instance.Identifier);

    public static string SettingsFilePath { get; } = System.IO.Path.Combine(AppDataFolderPath, "settings.json");

    public static string BackgroundFolderPath { get; } = System.IO.Path.Combine(AppDataFolderPath, "backgrounds");

    public static SettingsManager Instance { get; private set; } = new();

    public static SettingsManager Load()
    {
        if (!System.IO.File.Exists(SettingsFilePath) || string.IsNullOrWhiteSpace(System.IO.File.ReadAllText(SettingsFilePath)))
            return LoadDefaultSettings();

        try
        {
            var newSettings =
                JsonConvert.DeserializeObject<SettingsManager>(System.IO.File.ReadAllText(SettingsFilePath)) ?? new SettingsManager();

            Instance = newSettings;

            return newSettings;
        }
        catch (JsonException)
        {
            return LoadDefaultSettings();
        }
    }

    public static SettingsManager LoadDefaultSettings()
    {
        if (!System.IO.Directory.Exists(AppDataFolderPath)) System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(AppDataFolderPath)!);

        var defaultSettings = new SettingsManager();

        var jsonContent = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented);
        System.IO.File.WriteAllText(SettingsFilePath, jsonContent);

        return Instance = defaultSettings;
    }

    public void Save()
    {
        var jsonContent = JsonConvert.SerializeObject(this, Formatting.Indented);
        System.IO.File.WriteAllText(SettingsFilePath, jsonContent);
    }
}
