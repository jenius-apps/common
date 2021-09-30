using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using Windows.Globalization;

namespace JeniusApps.Common.Telemetry.Uwp
{
    public class AppCenterTelemetry : ITelemetry
    {
        public AppCenterTelemetry(string apiKey)
        {
            AppCenter.SetCountryCode(new GeographicRegion().CodeTwoLetter);
            AppCenter.Start(apiKey, typeof(Analytics), typeof(Crashes));
        }

        public void TrackError(Exception e, IDictionary<string, string> properties = null)
        {
            Crashes.TrackError(e, properties);
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null)
        {
            Analytics.TrackEvent(eventName, properties);
        }
    }
}
