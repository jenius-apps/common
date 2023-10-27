using Microsoft.Toolkit.Uwp.Notifications;
using System;
using Windows.UI.Notifications;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp;

public class ToastService : IToastService
{
    private readonly Lazy<IToastButton> _dismissButton;

    public ToastService()
    {
        _dismissButton = new Lazy<IToastButton>(() => new ToastButtonDismiss());
    }

    /// <inheritdoc/>
    public void ClearScheduledToasts()
    {
        try
        {
            ToastNotifierCompat notifier = ToastNotificationManagerCompat.CreateToastNotifier();
            var scheduled = notifier.GetScheduledToastNotifications();

            if (scheduled != null)
            {
                foreach (var toRemove in scheduled)
                {
                    notifier.RemoveFromSchedule(toRemove);
                }
            }
        }
        catch
        {
            // Crash telemetry suggests that sometimes, the 
            // "notification platform" is unavailable, leading to a crash somehere here.
            // We added the try-catch to try to mitigate the crash.
        }
    }

    /// <inheritdoc/>
    public void SendToast(
        string title,
        string message,
        bool dismissVisible = false,
        DateTime? scheduledDateTime = null,
        string launchArg = "",
        string tag = "",
        Uri? audioUri = null,
        bool audioSilent = false,
        int minutesExpiration = 0)
    {
        if (scheduledDateTime is not null &&
            scheduledDateTime <= DateTime.Now)
        {
            return;
        }

        var builder = new ToastContentBuilder()
            .SetToastScenario(ToastScenario.Default)
            .AddText(title)
            .AddText(message)
            .AddArgument(launchArg);

        if (audioUri is not null)
        {
            builder.AddAudio(audioUri, silent: audioSilent);
        }

        if (dismissVisible)
        {
            builder.AddButton(_dismissButton.Value);
        }

        if (scheduledDateTime is DateTime scheduledTime)
        {
            builder.Schedule(scheduledTime, toast =>
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    toast.Tag = tag;
                }

                if (minutesExpiration > 0)
                {
                    toast.ExpirationTime = DateTimeOffset.Now.AddMinutes(minutesExpiration);
                }
            });
        }
        else
        {
            builder.Show(toast =>
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    toast.Tag = tag;
                }
                
                if (minutesExpiration > 0)
                {
                    toast.ExpirationTime = DateTimeOffset.Now.AddMinutes(minutesExpiration);
                }
            });
        }
    }

    /// <inheritdoc/>
    public bool DoesToastExist(string toastTag)
    {
        var toasts = ToastNotificationManager.History.GetHistory();
        foreach (var toast in toasts)
        {
            if (toast.Tag == toastTag)
            {
                return true;
            }
        }

        return false;
    }
}
