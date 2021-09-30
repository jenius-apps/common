using System;
using System.Collections.Generic;
using Windows.Storage;

namespace JeniusApps.Common.Settings.Uwp
{
    public class LocalSettings : IUserSettings
    {
        private readonly IDictionary<string, object> _defaults;

        /// <inheritdoc/>
        public event EventHandler<string> SettingSet;

        public LocalSettings(IDictionary<string, object> defaults)
        {
            _defaults = defaults ?? new Dictionary<string, object>();
        }

        /// <inheritdoc/>
        public T Get<T>(string settingKey)
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
        public T Get<T>(string settingKey, T defaultOverride)
        {
            object result = ApplicationData.Current.LocalSettings.Values[settingKey];
            return result is null ? defaultOverride : (T)result;
        }

        private T GetDefault<T>(string settingKey)
        {
            return _defaults.ContainsKey(settingKey)
                ? (T)_defaults[settingKey]
                : default;
        }
    }
}
