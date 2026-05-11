using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Threading;
using SkEditor.API;
using CustomBackgroundAddon.Core;

namespace CustomBackgroundAddon.Background;

public class BackgroundManager
{
    public static void Register()
    {
        var settings = Settings.SettingsManager.Instance;

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (!BackgroundImageRenderer.Enabled()) return;
            BackgroundImageRenderer.Load();

            if (!settings.KeepEditorBackground)
            {
                var opacity = (int)Settings.SettingsManager.Instance.BackgroundOpacity;
                var hexValue = (int)(opacity / 100.0 * 255);
                var opacityHex = hexValue.ToString("X2");
                var backgroundBrush = new ImmutableSolidColorBrush(Color.Parse($"#{opacityHex}ffffff"));
                var transparentBrush = new ImmutableSolidColorBrush(Color.Parse("#00ffffff"));

                Core.StyleOverrides.Apply("EditorBackgroundColor", transparentBrush);
                Core.StyleOverrides.Apply("TabViewSelectedItemBorderBrush", backgroundBrush);
                Core.StyleOverrides.Apply("TabViewItemHeaderBackgroundSelected", backgroundBrush);
                Core.StyleOverrides.Apply("SkEditorBorderBackground", backgroundBrush);
            }
        });
    }

    public static void Unregister()
    {
        BackgroundImageRenderer.Remove();
        Core.StyleOverrides.RemoveAll();
    }

    public static void Reload()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            BackgroundImageRenderer.Remove();
            Core.StyleOverrides.RemoveAll();
            Register();
        });
    }

    public static void Setup()
    {
        BackgroundImageRenderer.Setup();
    }
}
