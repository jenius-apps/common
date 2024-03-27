using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace JeniusApps.Common.Telemetry;

/// <summary>
/// Class for capturing telemetry using
/// Microsoft Application Insights.
/// </summary>
public class AppInsightsTelemetry : ITelemetry
{
    private readonly TelemetryClient _tc;
    private bool _isEnabled = true;

    /// <summary>
    /// Initializes class.
    /// </summary>
    /// <param name="apiKey">The instrumentation key your AppInsights instance.</param>
    /// <param name="isEnabled">Determines if events will be tracked.</param>
    public AppInsightsTelemetry(
        string apiKey,
        bool isEnabled = true)
    {
        _isEnabled = isEnabled;

        var configuration = new TelemetryConfiguration
        {
            ConnectionString = $"InstrumentationKey={apiKey}", 
        };
        _tc = new TelemetryClient(configuration);
    }

    /// <inheritdoc/>
    public async Task FlushAsync()
    {
        await _tc.FlushAsync(default).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void SetEnabled(bool isEnabled) => _isEnabled = isEnabled; 

    /// <inheritdoc/>
    public void TrackError(
        Exception e,
        IDictionary<string, string>? properties = null,
        IDictionary<string, double>? metrics = null)
    {
        _tc.TrackException(e, properties, metrics);
    }

    /// <inheritdoc/>
    public void TrackEvent(string eventName,
        IDictionary<string, string>? properties = null,
        IDictionary<string, double>? metrics = null)
    {
        if (!_isEnabled)
        {
            return;
        }

        _tc.TrackEvent(eventName, properties, metrics);
    }
}
