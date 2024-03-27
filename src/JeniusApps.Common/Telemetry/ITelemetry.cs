using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeniusApps.Common.Telemetry;

/// <summary>
/// Telemetry interface.
/// </summary>
public interface ITelemetry
{
    /// <summary>
    /// Tracks exceptions.
    /// </summary>
    /// <param name="e">The exception to forward.</param>
    /// <param name="properties">Optional properties associated with the exception.</param>
    /// <param name="metrics">Optional metrics associated with the exception.</param>
    void TrackError(
        Exception e,
        IDictionary<string, string>? properties = null,
        IDictionary<string, double>? metrics = null);

    /// <summary>
    /// Tracks the given event and its properties.
    /// </summary>
    /// <param name="eventName">Name of event.</param>
    /// <param name="properties">Optional properties associated with the event.</param>
    /// <param name="metrics">Optional metrics associated with the event.</param>
    void TrackEvent(
        string eventName,
        IDictionary<string, string>? properties = null,
        IDictionary<string, double>? metrics = null);

    /// <summary>
    /// Sets if usage telemetry is enabled or not.
    /// </summary>
    /// <param name="isEnabled">If true, telemetry is enabled. If falsed, disabled.</param>
    void SetEnabled(bool isEnabled);

    /// <summary>
    /// Used to flush data and to avoid lost telemetry.
    /// Recommended to be used when application is shutting down
    /// or suspending.
    /// </summary>
    Task FlushAsync();
}
