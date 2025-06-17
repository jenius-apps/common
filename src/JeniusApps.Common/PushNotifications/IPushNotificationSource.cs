using System.Threading;
using System.Threading.Tasks;

namespace JeniusApps.Common.PushNotifications;

/// <summary>
/// Interface for retrieving push data from a Push Notification platform.
/// </summary>
public interface IPushNotificationSource
{
    /// <summary>
    /// Registers for push notifications with the source platform
    /// and retrieves the notification URI.
    /// </summary>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>If successful, returns a URI to be used for the push notification. Otherwise returns null.</returns>
    Task<string?> GetNotificationUriAsync(CancellationToken ct);
}
