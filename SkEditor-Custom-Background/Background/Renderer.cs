using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using SkEditor.API;
using SkEditor.Utilities.Styling;
using CustomBackgroundAddon.Settings;

namespace CustomBackgroundAddon.Background;

public static class BackgroundImageRenderer
{
    private static IBrush? _originalBackground;
    private static Bitmap? _currentBitmap;

    public static void Load()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();
        if (mainWindow == null)
        {
            SkEditorAPI.Logs.Warning("MainWindow is null, cannot load background image.");
            return;
        }

        var filePath = System.IO.Path.Combine(Settings.SettingsManager.Instance.CurrentBackgroundPath);
        if (!System.IO.File.Exists(filePath))
        {
            SkEditorAPI.Logs.Warning("File not found: " + filePath);
            return;
        }

        _originalBackground ??= mainWindow.Background;

        _ = Dispatcher.UIThread.InvokeAsync(() =>
        {
            var backgroundBlur = (float)Settings.SettingsManager.Instance.BackgroundBlur;

            var blurredBitmap = Blur.ApplyBlur(filePath, backgroundBlur);
            if (blurredBitmap == null)
            {
                SkEditorAPI.Logs.Warning("Failed to decode image: " + filePath);
                return;
            }

            _currentBitmap?.Dispose();
            _currentBitmap = blurredBitmap;

            mainWindow.Background = new ImageBrush(blurredBitmap)
            {
                Stretch = Stretch.UniformToFill,
                AlignmentX = AlignmentX.Center,
                AlignmentY = AlignmentY.Center,
            };
        });
    }

    public static void Remove()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();
        if (mainWindow == null) return;
        Dispatcher.UIThread.Post(() =>
        {
            mainWindow.Background = ThemeEditor.CurrentTheme.BackgroundColor;
        });

        _currentBitmap?.Dispose();
        _currentBitmap = null;
        _originalBackground = null;
    }

    public static bool Enabled()
    {
        return Settings.SettingsManager.Instance.CurrentBackgroundPath != "";
    }

    public static void Setup()
    {
        var directoryExists = System.IO.Directory.Exists(Settings.SettingsManager.BackgroundFolderPath);
        if (!directoryExists) System.IO.Directory.CreateDirectory(Settings.SettingsManager.BackgroundFolderPath);
    }
}
