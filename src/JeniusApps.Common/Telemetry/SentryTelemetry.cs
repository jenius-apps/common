using Sentry;
using System;
using System.Collections.Generic;

namespace JeniusApps.Common.Telemetry;

/// <summary>
/// Class for capturing telemetry using Sentry.
/// </summary>
public class SentryTelemetry : ITelemetry
{
    private bool _isEnabled = true;

    /// <summary>
    /// Initializes class.
    /// </summary>
    /// <param name="apiKey">The Data Source Name for the Sentry options.</param>
    /// <param name="isEnabled">Determines if telemetry will be sent or not.</param>
    public SentryTelemetry(string apiKey, bool isEnabled = true)
    {
        _isEnabled = isEnabled;

        SentrySdk.Init(options =>
        {
            options.Dsn = apiKey;
            options.ExperimentalMetrics = new ExperimentalMetricsOptions
            {
                // Used for the Metrics feature which is currently a beta.
                EnableCodeLocations = true 
            };

            // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
            options.TracesSampleRate = 1.0;

            // Enable Global Mode since this is a client app.
            options.IsGlobalModeEnabled = true;

            // Recommended to be enabled for desktop client apps.
            options.AutoSessionTracking = true;
        });
    }

    /// <inheritdoc/>
    public void SetEnabled(bool isEnabled)
    {
        _isEnabled = isEnabled;
    }

    /// <inheritdoc/>
    public void TrackError(Exception e, IDictionary<string, string>? properties = null)
    {
        if (!_isEnabled)
        {
            return;
        }

        SentrySdk.CaptureException(e);
        _ = SentrySdk.FlushAsync(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void TrackEvent(string eventName, IDictionary<string, string>? properties = null)
    {
        if (!_isEnabled)
        {
            return;
        }

        SentrySdk.Metrics.Increment(eventName, tags: properties);
    }
}
