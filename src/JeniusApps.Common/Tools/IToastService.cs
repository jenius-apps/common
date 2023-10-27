using System;

namespace JeniusApps.Common.Tools;

/// <summary>
/// Service for sending toasts.
/// </summary>
public interface IToastService
{
    /// <summary>
    /// Clears the scheduled toasts.
    /// </summary>
    void ClearScheduledToasts();

    /// <summary>
    /// Pops the toast based on the given time.
    /// If no time is provided, the toast will pop immediately.
    /// </summary>
    /// <param name="title">Title of the toast.</param>
    /// <param name="message">Message body of the toast.</param>
    /// <param name="dismissVisible">Adds dismiss button if true.</param>
    /// <param name="scheduledDateTime">The time when the notification will be triggered.</param>
    /// <param name="launchArg">Arguments that will be added to the toast. This is passed onto the app foreground when the toast is clicked.</param>
    /// <param name="tag">A unique ID assigned to the toast that can be used for searching the toast later.</param>
    /// <param name="audioUri">URI for an audio file that will be used as the notification sound.</param>
    /// <param name="audioSilent">If true, the notification will not make a sound. Only used when an audioUri is provided.</param>
    /// <param name="minutesExpiration">Defines how long in minutes that the toast will remain active. After expiration, the toast disappears. A value of 0 minutes no expiration.</param>
    void SendToast(
        string title,
        string message,
        bool dismissVisible = false,
        DateTime? scheduledDateTime = null,
        string launchArg = "",
        string tag = "",
        Uri? audioUri = null,
        bool audioSilent = false,
        int minutesExpiration = 0);

    /// <summary>
    /// Determines if the toast exists already.
    /// </summary>
    /// <param name="toastTag">A unique ID assigned to the toast that can be used for searching the toast later.</param>
    /// <returns>True if the toast already exists.</returns>
    bool DoesToastExist(string toastTag);

}
