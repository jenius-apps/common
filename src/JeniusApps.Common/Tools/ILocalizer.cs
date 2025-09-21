namespace JeniusApps.Common.Tools
{
    /// <summary>
    /// Interfaces for retrieving localized strings.
    /// </summary>
    public interface ILocalizer
    {
        /// <summary>
        /// Retrieves the localized string for the given key.
        /// </summary>
        /// <param name="key">The resource key for the string.</param>
        string GetString(string key);

        /// <summary>
        /// Retrieves the localized string for the given key and uses
        /// the given parameter to format the localized string.
        /// </summary>
        /// <param name="key">The resource key for the string.</param>
        /// <param name="formatParam">The parameter to use to format the localized string.</param>
        string GetString(string key, string formatParam);

        /// <summary>
        /// Retrieves the localized string for the given key and uses
        /// the given parameters to format the localized string.
        /// </summary>
        /// <param name="key">The resource key for the string.</param>
        /// <param name="formatParams">The parameters to use to format the localized string.</param>
        string FormatString(string key, params string[] formatParams);
    }
}
