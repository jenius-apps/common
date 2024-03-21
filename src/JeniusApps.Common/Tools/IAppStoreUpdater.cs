using System;
using System.Threading.Tasks;

namespace JeniusApps.Common.Tools;

/// <summary>
/// Interface for handling app store updates
/// from within your application.
/// </summary>
public interface IAppStoreUpdater
{
    /// <summary>
    /// Raised when an update is detected.
    /// </summary>
    event EventHandler? UpdateAvailable;

    /// <summary>
    /// Raised when an update is in progress. The double
    /// payload is the percent progress of the update.
    /// </summary>
    event EventHandler<double>? ProgressChanged;

    /// <summary>
    /// Checks if updates exist for the current app.
    /// If updates exist, the update information will be cached,
    /// but it does not perform the updates.
    /// </summary>
    /// <returns>True if updates are available, false otherwise.</returns>
    public Task<bool> CheckForUpdatesAsync();

    /// <summary>
    /// Attempts to apply updates that were previously
    /// cached using <see cref="CheckForUpdatesAsync"/>.
    /// If the cache is empty, no operation is performed.
    /// </summary>
    public Task<bool?> TryApplyUpdatesAsync();

    /// <summary>
    /// Attempts to silently download and install the update
    /// in the background. 
    /// </summary>
    /// <remarks>
    /// If the update was downloaded previously, then we skip to the install step.
    /// The installation step will restart the app.
    /// </remarks>
    /// <returns>Returns false if the download was not completed successfully.</returns>
    Task<bool> TrySilentDownloadAndInstallAsync();

    /// <summary>
    /// Attemps to silently download the update in the background, but
    /// the install step will not be triggered.
    /// </summary>
    /// <returns>Returns false if the download was not completed successfully.</returns>
    Task<bool> TrySilentDownloadAsync();
}
