using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
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
    /// <param name="context">Optional context to add to the telemetry client.</param>
    public AppInsightsTelemetry(
        string apiKey,
        bool isEnabled = true,
        TelemetryContext? context = null)
    {
        _isEnabled = isEnabled;

        var configuration = new TelemetryConfiguration
        {
            ConnectionString = $"InstrumentationKey={apiKey}"
            
        };
        _tc = new TelemetryClient(configuration);

        if (context is not null)
        {
            _tc.Context.Component.Version = context.Component.Version;
            _tc.Context.Device.Id = context.Device.Id;
            _tc.Context.Device.OperatingSystem = context.Device.OperatingSystem;
            _tc.Context.Location.Ip = context.Location.Ip;
            _tc.Context.Session.Id = context.Session.Id;
            _tc.Context.Session.IsFirst = context.Session.IsFirst;
            _tc.Context.User.Id = context.User.Id;
            _tc.Context.User.AuthenticatedUserId = context.User.AuthenticatedUserId;
            
            foreach (var property in context.GlobalProperties)
            {
                _tc.Context.GlobalProperties.Add(property.Key, property.Value);
            }
        }
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

    /// <inheritdoc/>
    public void TrackPageView(string page)
    {
        if (!_isEnabled)
        {
            return;
        }

        _tc.TrackPageView(page);
    }
}
