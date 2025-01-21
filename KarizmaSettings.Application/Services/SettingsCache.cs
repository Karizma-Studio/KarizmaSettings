using KarizmaPlatform.Settings.Domain.Models;

namespace KarizmaPlatform.Settings.Application.Services;

public class SettingsCache
{
    private readonly Dictionary<string, string> settingsDictionary = new();

    public void Populate(List<Setting> settings)
    {
        settingsDictionary.Clear();

        foreach (var setting in settings)
        {
            settingsDictionary.Add(GetTypeKey(setting.Type, setting.Key), setting.Value);
        }
    }

    public T GetResource<T>(string type, string key)
    {
        try
        {
            var value = settingsDictionary[GetTypeKey(type, key)];
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch (Exception e)
        {
            throw new InvalidCastException($"Unable to convert value to type {typeof(T)}.", e);
        }
    }

    private string GetTypeKey(string type, string key)
    {
        return $"{type}_{key}";
    }
}