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
    /// <param name="logLevel">The log level of the telemetry.</param>
    void TrackError(
        Exception e,
        IDictionary<string, string>? properties = null,
        IDictionary<string, double>? metrics = null,
        LogLevel logLevel = LogLevel.Critical);

    /// <summary>
    /// Tracks the given event and its properties.
    /// </summary>
    /// <param name="eventName">Name of event.</param>
    /// <param name="properties">Optional properties associated with the event.</param>
    /// <param name="metrics">Optional metrics associated with the event.</param>
    /// <param name="logLevel">The log level of the telemetry.</param>
    void TrackEvent(
        string eventName,
        IDictionary<string, string>? properties = null,
        IDictionary<string, double>? metrics = null,
        LogLevel logLevel = LogLevel.Basic);

    /// <summary>
    /// Sets if usage telemetry is enabled or not.
    /// </summary>
    /// <param name="isEnabled">If true, telemetry is enabled. If falsed, disabled.</param>
    void SetEnabled(bool isEnabled);

    /// <summary>
    /// Sets the minimum logging level.
    /// </summary>
    /// <param name="logLevel">The minimum log level to record.</param>
    void SetMinimumLogLevel(LogLevel logLevel);

    /// <summary>
    /// Used to flush data and to avoid lost telemetry.
    /// Recommended to be used when application is shutting down
    /// or suspending.
    /// </summary>
    Task FlushAsync();

    /// <summary>
    /// Tracks the page view event.
    /// </summary>
    /// <param name="page">Name of the page.</param>
    /// <param name="logLevel">The log level of the telemetry.</param>
    void TrackPageView(string page, LogLevel logLevel = LogLevel.Basic);
}

/// <summary>
/// Describes the logging levels for telemetry.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Basic telemetry that isn't business critical.
    /// </summary>
    Basic,

    /// <summary>
    /// Business critical telemetry that will be logged.
    /// </summary>
    Critical
}
