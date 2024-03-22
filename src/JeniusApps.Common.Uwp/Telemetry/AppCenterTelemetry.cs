using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using Windows.Globalization;

namespace JeniusApps.Common.Telemetry.Uwp;

public class AppCenterTelemetry : ITelemetry
{
    private bool _isEnabled = true;

    public AppCenterTelemetry(string apiKey, bool isEnabled = true)
    {
        _isEnabled = isEnabled;
        AppCenter.SetCountryCode(new GeographicRegion().CodeTwoLetter);
        AppCenter.Start(apiKey, typeof(Analytics), typeof(Crashes));
    }

    public void SetEnabled(bool isEnabled)
    {
        _isEnabled = isEnabled;
    }

    public void TrackError(Exception e, IDictionary<string, string> properties = null)
    {
        if (!_isEnabled)
        {
            return;
        }

        Crashes.TrackError(e, properties);
    }

    public void TrackEvent(string eventName, IDictionary<string, string> properties = null)
    {
        if (!_isEnabled)
        {
            return;
        }

        Analytics.TrackEvent(eventName, properties);
    }
}
