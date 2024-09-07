using JeniusApps.Common.Settings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JeniusApps.Common.Tools;

/// <summary>
/// Implementation of an experimentation service that uses local settings.
/// </summary>
public class LocalExperimentationService : IExperimentationService
{
    private readonly IReadOnlyList<string> _activeExperiments;
    private readonly IUserSettings _userSettings;
    private readonly Random _rand = new();
    private readonly ConcurrentDictionary<string, bool> _cachedResults = new();
    private readonly object _dictionaryLock = new();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="activeExperiments">List of active experimentation keys.</param>
    /// <param name="userSettings">Required user settings service.</param>
    public LocalExperimentationService(
        IReadOnlyList<string> activeExperiments,
        IUserSettings userSettings)
    {
        _activeExperiments = activeExperiments;
        _userSettings = userSettings;
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, bool> GetAllExperiments()
    {
        var result = new Dictionary<string, bool>();
        foreach (var experiment in _activeExperiments)
        {
            result[experiment] = IsEnabled(experiment);
        }

        return result;
    }

    /// <inheritdoc/>
    public bool IsEnabled(string experiment)
    {
        lock (_dictionaryLock)
        {
            if (_cachedResults.TryGetValue(experiment, out bool value))
            {
                return value;
            }

            var key = $"experimentation-{experiment}";
            var newOrStoredValue = _userSettings.Get(key, _rand.Next(0, 2) == 0);
            _cachedResults.TryAdd(experiment, newOrStoredValue);
            _userSettings.Set(key, newOrStoredValue);
            return newOrStoredValue;
        }
    }
}
