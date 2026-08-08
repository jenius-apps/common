using System;
using System.Threading.Tasks;

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

        /// <summary>
        /// Gets a value indicating whether the app is being used for the first time since being upgraded from an older version.
        /// Use this to tell if you should display details about what has changed.
        /// </summary>
        bool WasAppUpdated();

        /// <summary>
        /// App version when this app instance was first installed.
        /// </summary>
        string FirstInstalledAppVersion();

        /// <summary>
        /// Previous app version.
        /// </summary>
        string PreviousAppVersion();

        /// <summary>
        /// Current app version.
        /// </summary>
        string CurrentAppVersion();

        /// <summary>
        /// Returns a bool representing whether the given app is installed.
        /// </summary>
        /// <param name="packageFamilyName">The package family name of the app we are checking.</param>
        Task<bool> IsAppInstalledAsync(string packageFamilyName);

        /// <summary>
        /// Gets a value indicating whether if the current internet connection is metered.
        /// </summary>
        bool IsInternetOnMeteredConnection();

        /// <summary>
        /// Gets a value indicating whether internet is available across all connections.
        /// </summary>
        bool IsInternetAvailable();
    }
}
