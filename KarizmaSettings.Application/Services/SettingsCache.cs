using KarizmaPlatform.Settings.Domain.Models;
using Microsoft.Extensions.Logging;

namespace KarizmaPlatform.Settings.Application.Services;

public class SettingsCache(ILogger<SettingsCache> logger)
{
    private readonly Dictionary<string, string> settingsDictionary = new();
    private List<Setting> availableSettings = [];

    public void Populate(List<Setting> settings)
    {
        settingsDictionary.Clear();
        availableSettings.Clear();

        availableSettings = settings;

        foreach (var setting in availableSettings)
        {
            settingsDictionary.Add(GetTypeKey(setting.Type, setting.Key), setting.Value);
        }
    }

    public T GetValue<T>(string type, string key, T defaultValue)
    {
        try
        {
            if (settingsDictionary.TryGetValue(GetTypeKey(type, key), out var value))
            {
                if (typeof(T) == typeof(TimeSpan))
                    return (T)(object)TimeSpan.Parse(value);

                return (T)Convert.ChangeType(value, typeof(T));
            }
            else
            {
                return defaultValue;
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unable to convert value to type {typeof(T)}.");
            return defaultValue;
        }
    }

    public List<Setting> GetAllPublicSettings()
    {
        return availableSettings.Where(s => s.IsPublic).ToList();
    }

    public List<Setting> GetAllSettings()
    {
        return availableSettings;
    }

    private string GetTypeKey(string type, string key)
    {
        return $"{type}_{key}";
    }
}