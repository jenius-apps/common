using System;

namespace JeniusApps.Common.Settings
{
    /// <summary>
    /// Interface for storing
    /// and retrieving user settings.
    /// </summary>
    public interface IUserSettings
    {
        /// <summary>
        /// Raised when a settings is set.
        /// String is the key of the setting.
        /// </summary>
        event EventHandler<string> SettingSet;

        /// <summary>
        /// Saves settings into persistent local
        /// storage.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="settingKey">The settings key to use.</param>
        /// <param name="value">The value to save.</param>
        void Set<T>(string settingKey, T value);

        /// <summary>
        /// Retrieves the value for the desired settings key.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="settingKey">The settings key to use.</param>
        /// <returns>The desired value or returns the default value.</returns>
        T Get<T>(string settingKey);

        /// <summary>
        /// Retrieves the value for the desired settings key.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="settingKey">The settings key to use.</param>
        /// <param name="defaultOverride">The default override to use if the setting has no value.</param>
        /// <returns>The desired value or returns the default override.</returns>
        T Get<T>(string settingKey, T defaultOverride);
    }
}
