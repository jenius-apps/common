using System.Collections.Generic;

namespace JeniusApps.Common.Tools;

/// <summary>
/// Interface for an experimentation service.
/// </summary>
public interface IExperimentationService
{
    /// <summary>
    /// Returns the local state of all active experiments.
    /// </summary>
    /// <returns>
    /// A dictionary for each experiment and a bool that represents if they 
    /// should be enabled or not.
    /// </returns>
    IReadOnlyDictionary<string, bool> GetAllExperiments();

    /// <summary>
    /// Determines if the experiment should be enabled.
    /// </summary>
    /// <param name="experiment">The unique name of the experiment.</param>
    /// <returns>Returns true if the experiment should be enabled.</returns>
    bool IsEnabled(string experiment);
}