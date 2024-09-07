using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Windows.Storage;

#nullable enable

namespace JeniusApps.Common.Settings.Uwp;

public class LocalSettings : IUserSettings
{
    private readonly IReadOnlyDictionary<string, object> _defaults;

    /// <inheritdoc/>
    public event EventHandler<string>? SettingSet;

    public LocalSettings(IReadOnlyDictionary<string, object> defaults)
    {
        _defaults = defaults;
    }

    /// <inheritdoc/>
    public T? Get<T>(string settingKey)
    {
        object result = ApplicationData.Current.LocalSettings.Values[settingKey];
        return result is null ? GetDefault<T>(settingKey) : (T)result;
    }

    /// <inheritdoc/>
    public void Set<T>(string settingKey, T value)
    {
        ApplicationData.Current.LocalSettings.Values[settingKey] = value;
        SettingSet?.Invoke(this, settingKey);
    }

    /// <inheritdoc/>
    public T? Get<T>(string settingKey, T defaultOverride)
    {
        object result = ApplicationData.Current.LocalSettings.Values[settingKey];
        return result is null ? defaultOverride : (T)result;
    }

    /// <inheritdoc/>
    public T? GetAndDeserialize<T>(string settingKey, JsonTypeInfo<T> jsonTypeInfo)
    {
        object result = ApplicationData.Current.LocalSettings.Values[settingKey];
        if (result is string serialized)
        {
            try
            {
                return JsonSerializer.Deserialize(serialized, jsonTypeInfo);
            }
            catch { }
        }

        return GetDefault<T>(settingKey);
    }

    /// <inheritdoc/>
    public void SetAndSerialize<T>(string settingKey, T value, JsonTypeInfo<T> jsonTypeInfo)
    {
        var serialized = JsonSerializer.Serialize(value, jsonTypeInfo);
        Set(settingKey, serialized);
    }

    private T? GetDefault<T>(string settingKey)
    {
        return _defaults.ContainsKey(settingKey)
            ? (T)_defaults[settingKey]
            : default;
    }
}
