using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;

#nullable enable

namespace JeniusApps.Common.PushNotifications.Uwp;

/// <summary>
/// An implementation of the notification source for the Windows Notification Service.
/// </summary>
public sealed class WindowsPushNotificationSource : IPushNotificationSource
{
    /// <inheritdoc/>
    public async Task<string?> GetNotificationUriAsync(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        try
        {
            PushNotificationChannel? channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            return channel?.Uri;
        }
        catch
        {
            return null;
        }        
    }
}
