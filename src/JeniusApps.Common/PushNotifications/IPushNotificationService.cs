using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JeniusApps.Common.PushNotifications;

/// <summary>
/// Main interface for orchestrating push notification registration.
/// </summary>
public interface IPushNotificationService
{
    /// <summary>
    /// Registers the given device for push notifications.
    /// </summary>
    /// <param name="deviceId">A unique ID representing this device. Preferably a GUID.</param>
    /// <param name="primaryLanguageCode">A two-letter ISO language code representing the user's primary language.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <param name="deviceData">Optional. Device data that can be used for targeting.</param>
    /// <returns>True if successful, false otherwise.</returns>
    Task<bool> RegisterAsync(
        string deviceId,
        string primaryLanguageCode,
        CancellationToken ct,
        Dictionary<string, string>? deviceData = null);

    /// <summary>
    /// Unregisters the given device from notifications.
    /// </summary>
    /// <param name="deviceId">A unique ID representing this device. Preferably a GUID.</param>
    /// <param name="ct">A cancellation token.</param>
    Task UnregisterAsync(string deviceId, CancellationToken ct);
}