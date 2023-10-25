using System.Threading.Tasks;

namespace JeniusApps.Common.Tools;

/// <summary>
/// Interface for handling startup-related logic.
/// </summary>
public interface IStartupService
{
    /// <summary>
    /// Gets the current state of the startup task.
    /// </summary>
    /// <param name="startupTaskId">The ID associated with the app's startup.</param>
    /// <returns></returns>
    Task<StartupState> GetStateAsync(string startupTaskId);

    /// <summary>
    /// Opens system dialog requesting permission to turn on the given startup task.
    /// Must be called from the UI thread.
    /// </summary>
    /// <param name="startupTaskId">The ID associated with the app's startup.</param>
    /// <returns>True if the state is enabled, false otherwise.</returns>
    Task<bool> TryEnableOnUiThreadAsync(string startupTaskId);
}

/// <summary>
/// State related to the app's startup task.
/// </summary>
public enum StartupState
{
    /// <summary>
    /// Startup is disabled but it can be enabled.
    /// </summary>
    Disabled,

    /// <summary>
    /// Startup is disabled and must be enabled manually from Windows Settings.
    /// </summary>
    DisabledByUser,

    /// <summary>
    /// Not allowed due to group policy or the device does not support it.
    /// </summary>
    Disallowed,

    /// <summary>
    /// Startup is already enabled.
    /// </summary>
    Enabled,
}
