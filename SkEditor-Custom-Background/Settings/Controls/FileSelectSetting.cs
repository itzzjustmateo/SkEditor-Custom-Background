using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using Newtonsoft.Json.Linq;
using SkEditor.API;
using SkEditor.API.Settings;
using SkEditor.API.Settings.Types;
using SkEditor.Views.Settings;
using CustomBackgroundAddon.Core;
using Symbol = FluentIcons.Common.Symbol;
using SymbolIcon = FluentIcons.Avalonia.SymbolIcon;

namespace CustomBackgroundAddon.Settings.Controls;

public class FileSelectSetting : ISettingType
{
    public object Deserialize(JToken value)
    {
        throw new NotImplementedException();
    }

    public JToken Serialize(object value)
    {
        throw new NotImplementedException();
    }

    public Control CreateControl(object value, Action<object> onChanged)
    {
        var stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };

        var comboBox = new ComboBox { PlaceholderText = "Select" };
        comboBox.Items.Add(new ComboBoxItem()
        {
            Content = "None",
            Tag = "none"
        });

        var files = System.IO.Directory.GetFiles(SettingsManager.BackgroundFolderPath);
        var selectedIndex = 0;

        for (var i = 0; i < files.Length; i++)
        {
            var comboBoxItem = new ComboBoxItem
            {
                Content = System.IO.Path.GetFileName(files[i]),
                Tag = Uri.UnescapeDataString(files[i])
            };

            comboBox.Items.Add(comboBoxItem);

            if (files[i] == SettingsManager.Instance.CurrentBackgroundPath)
            {
                selectedIndex = i + 1;
            }
        }

        comboBox.SelectedIndex = selectedIndex;

        comboBox.SelectionChanged += (_, _) =>
        {
            var selectedPath = (comboBox.SelectedItem as ComboBoxItem)?.Tag as string;
            if (selectedPath == "none") selectedPath = null;

            SettingsManager.Instance.CurrentBackgroundPath = selectedPath ?? "";
            SettingsManager.Instance.Save();
            CustomAddonSettingsPage.Load(CustomBackgroundAddon.Instance);

            Background.BackgroundManager.Reload();
        };

        var button = new Button
        {
            Content = new SymbolIcon() { Symbol = Symbol.Add },
            Command = new RelayCommand(async () =>
            {
                var filePath = await SkEditorAPI.Windows.AskForFile(new FilePickerOpenOptions()
                {
                    Title = Translation.Get("SettingsBackgroundImageSelectWindowTitle"),
                    AllowMultiple = false,
                    FileTypeFilter = [
                        new FilePickerFileType("Supported Image Formats")
                        {
                            Patterns = ["*.png", "*.jpg", "*.jpeg", "*.webp", "*.gif", "*.ico", "*.bmp"]
                        }
                    ]
                });

                if (filePath is null)
                {
                    return;
                }

                filePath = Uri.UnescapeDataString(filePath);

                System.IO.File.Copy(filePath, System.IO.Path.Join(SettingsManager.BackgroundFolderPath, System.IO.Path.GetFileName(filePath)), false);

                SettingsManager.Instance.CurrentBackgroundPath = filePath;
                SettingsManager.Instance.Save();
                CustomAddonSettingsPage.Load(CustomBackgroundAddon.Instance);
                Background.BackgroundManager.Reload();
            })
        };

        stackPanel.Children.Add(comboBox);
        stackPanel.Children.Add(button);

        return stackPanel;
    }

    public void SetupExpander(SettingsExpander expander, Setting setting)
    {
    }

    public bool IsSelfManaged => true;
}
