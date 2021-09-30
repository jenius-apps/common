using System;
using System.Collections.Generic;

namespace JeniusApps.Common.Telemetry
{
    public interface ITelemetry
    {
        /// <summary>
        /// Tracks exceptions.
        /// </summary>
        void TrackError(Exception e, IDictionary<string, string>? properties = null);

        /// <summary>
        /// Tracks the given event and its properties.
        /// </summary>
        /// <param name="eventName">Name of event.</param>
        /// <param name="properties">Optoinal properties associated with the event.</param>
        void TrackEvent(string eventName, IDictionary<string, string>? properties = null);
    }
}
