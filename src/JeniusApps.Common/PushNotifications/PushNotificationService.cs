using CommunityToolkit.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JeniusApps.Common.PushNotifications;

/// <summary>
/// Main orchestrator for registering push notifications.
/// </summary>
public class PushNotificationService : IPushNotificationService
{
    private readonly IPushNotificationSource _source;
    private readonly IPushNotificationStorage _storage;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="source">The source that provides the push notification URL for each registered device.</param>
    /// <param name="storage">The storage where the registrations are saved.</param>
    public PushNotificationService(
        IPushNotificationSource source,
        IPushNotificationStorage storage)
    {
        _source = source;
        _storage = storage;
    }

    /// <inheritdoc/>
    public async Task<bool> RegisterAsync(
        string deviceId,
        string primaryLanguageCode,
        CancellationToken ct,
        Dictionary<string, string>? deviceData = null)
    {
        ct.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(deviceId))
        {
            ThrowHelper.ThrowArgumentException(nameof(deviceId));
        }

        if (string.IsNullOrEmpty(primaryLanguageCode))
        {
            ThrowHelper.ThrowArgumentException(nameof(primaryLanguageCode));
        }

        string? uri = await _source.GetNotificationUriAsync(ct);
        if (uri is not { Length: > 0 })
        {
            return false;
        }

        DeviceRegistrationData data = new()
        {
            ActionRequested = "register",
            DeviceId = deviceId,
            PrimaryLanguageCode = primaryLanguageCode,
            Uri = uri,
            DeviceData = deviceData ?? []
        };

        return await _storage.RegisterDeviceAsync(data, ct);
    }

    /// <inheritdoc/>
    public async Task UnregisterAsync(string deviceId, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        await _storage.DeleteDeviceRegistrationAsync(deviceId, ct);
    }
}
