namespace JeniusApps.Common.Tools
{
    /// <summary>
    /// Interface for creating INavigator instances.
    /// </summary>
    public interface INavigatorFactory
    {
        /// <summary>
        /// Creates a new INavigator instance using the given parameters.
        /// </summary>
        /// <param name="navigatorName">Key name for the navigator. Can be used to retrieve the existing instance after it's been created.</param>
        /// <param name="constructorParameter">The parameter to be supplied to the constructor of the INavigator instance if construction is needed.</param>
        /// <param name="frame">The frame object to be associated with the INavigator instance. Will always override the previous frame object.</param>
        /// <returns>Returns the newly created INavigator instance.</returns>
        INavigator GetOrCreate(string navigatorName, object? constructorParameter, object? frame);

        /// <summary>
        /// Retrieves an INavigator instance with the given navigator key name.
        /// </summary>
        /// <param name="navigatorName">The key name of the INavigator to fetch.</param>
        /// <returns>Returns the requested INavigator instance or null if it doesn't exist.</returns>
        INavigator? Get(string navigatorName);
    }
}
