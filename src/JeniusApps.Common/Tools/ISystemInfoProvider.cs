using System;

namespace JeniusApps.Common.Tools
{
    /// <summary>
    /// Series of methods that retrieve information about the system.
    /// </summary>
    public interface ISystemInfoProvider
    {
        /// <summary>
        /// Retrieves the culture name
        /// in en-US format.
        /// </summary>
        string GetCulture();

        /// <summary>
        /// Returns string representing the device family.
        /// </summary>
        string GetDeviceFamily();

        /// <summary>
        /// Returns true if the current
        /// session is the first time this app
        /// was run since being installed.
        /// </summary>
        bool IsFirstRun();

        /// <summary>
        /// Returns true if the app is currently in compact mode.
        /// </summary>
        bool IsCompact();

        /// <summary>
        /// Returns true if the system is capable of using
        /// the built-in fluent system icons.
        /// </summary>
        bool IsWin11();

        /// <summary>
        /// Returns the date time when the app was first used.
        /// </summary>
        /// <returns>DateTime when the app was first used.</returns>
        DateTime FirstUseDate();

        /// <summary>
        /// Returns the local folder path for application data.
        /// </summary>
        string LocalFolderPath();

        /// <summary>
        /// Returns true if the device's battery saver is active.
        /// </summary>
        bool IsOnBatterySaver();

        /// <summary>
        /// Returns true if the system settings is set to prefer left hand mode.
        /// Returs false if right hand mode.
        /// </summary>
        bool IsLeftHandPreference();
    }
}
