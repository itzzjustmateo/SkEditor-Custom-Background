using System.Reflection;
using CustomBackgroundAddon.Settings;

namespace CustomBackgroundAddon.Core;

public static class Resources
{
    public static void ExtractEmbededResource(string resourceName, string outputPath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) throw new FileNotFoundException(resourceName);

        using var fileStream = System.IO.File.Create(outputPath);
        stream.CopyTo(fileStream);
    }

    public static Stream StreamEmbededResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) throw new FileNotFoundException(resourceName);
        return stream;
    }

    public static void ExtractLanguages()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var embeddedResources = assembly.GetManifestResourceNames()
                                        .Where(name => name.StartsWith("CustomBackgroundAddon.Languages.") &&
                                                       name.EndsWith(".xaml"));
        foreach (var resourceName in embeddedResources)
        {
            var name = resourceName.Replace("CustomBackgroundAddon.Languages.", "").Replace(".xaml", "");
            var outputPath = System.IO.Path.Combine(Settings.SettingsManager.AppDataFolderPath, "Languages", $"{name}.xaml");
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath)!);
            ExtractEmbededResource(resourceName, outputPath);
        }
    }
}
