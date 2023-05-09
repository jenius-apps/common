using System;
using System.Collections.Generic;
using Windows.Storage;

#nullable enable

namespace JeniusApps.Common.Settings.Uwp
{
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

        private T? GetDefault<T>(string settingKey)
        {
            return _defaults.ContainsKey(settingKey)
                ? (T)_defaults[settingKey]
                : default;
        }
    }
}
