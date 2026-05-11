using Avalonia.Input;
using SkEditor.API;
using CustomBackgroundAddon.Settings;

namespace CustomBackgroundAddon.Core;

public static class Events
{
    public static void Register()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();

        if (mainWindow != null)
        {
            mainWindow.KeyDown += OnMainWindowKeyDown;
        }

        SkEditorAPI.Events.OnLanguageChanged += OnLanguageChanged;
        SkEditorAPI.Events.OnAddonSettingChanged += OnAddonSettingChanged;
    }

    public static void Unregister()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();

        if (mainWindow != null)
        {
            mainWindow.KeyDown -= OnMainWindowKeyDown;
        }

        SkEditorAPI.Events.OnLanguageChanged -= OnLanguageChanged;
        SkEditorAPI.Events.OnAddonSettingChanged -= OnAddonSettingChanged;
    }

    private static void OnLanguageChanged(object? sender, LanguageChangedEventArgs args)
    {
        Translation.ChangeLanguage(args.Language).Wait();
    }

    private static void OnMainWindowKeyDown(object? sender, KeyEventArgs args)
    {

    }

    private static void OnAddonSettingChanged(object? sender, AddonSettingChangedEventArgs args)
    {
        if (args.Setting.Addon.Identifier != CustomBackgroundAddon.Instance.Identifier) return;

        var key = args.Setting.Key;
        var newValue = CustomBackgroundAddon.Instance.GetSetting(key);

        if (newValue != null)
        {
            switch (key)
            {
                case "BackgroundBlur":
                    Settings.SettingsManager.Instance.BackgroundBlur = (double)newValue;
                    break;
                case "BackgroundOpacity":
                    Settings.SettingsManager.Instance.BackgroundOpacity = (double)newValue;
                    break;
                case "KeepEditorBackground":
                    Settings.SettingsManager.Instance.KeepEditorBackground = (bool)newValue;
                    break;
            }

            Settings.SettingsManager.Instance.Save();
        }

        Background.BackgroundManager.Reload();
    }
}
