using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Newtonsoft.Json.Linq;
using SkEditor.API.Settings;
using SkEditor.API.Settings.Types;

namespace CustomBackgroundAddon.Settings.Controls;

public class InfoSetting : ISettingType
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
        return new Label { Content = "Value: " + value };
    }

    public void SetupExpander(SettingsExpander expander, Setting setting)
    {
    }

    public bool IsSelfManaged => false;
}
