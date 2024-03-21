using Sentry;
using System;
using System.Collections.Generic;

namespace JeniusApps.Common.Telemetry;

/// <summary>
/// Class for capturing telemetry using Sentry.
/// </summary>
public class SentryTelemetry : ITelemetry
{
    /// <summary>
    /// Initializes class.
    /// </summary>
    /// <param name="apiKey">The Data Source Name for the Sentry options.</param>
    public SentryTelemetry(string apiKey)
    {
        SentrySdk.Init(options =>
        {
            options.Dsn = apiKey;
            options.ExperimentalMetrics = new ExperimentalMetricsOptions
            {
                EnableCodeLocations = true
            };

            // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
            options.TracesSampleRate = 1.0;

            // Enable Global Mode since this is a client app.
            options.IsGlobalModeEnabled = true;
        });
    }

    /// <inheritdoc/>
    public void TrackError(Exception e, IDictionary<string, string>? properties = null)
    {
        SentrySdk.CaptureException(e);
        _ = SentrySdk.FlushAsync(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void TrackEvent(string eventName, IDictionary<string, string>? properties = null)
    {
        SentrySdk.Metrics.Increment(eventName, tags: properties);
    }
}
