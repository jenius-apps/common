using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace JeniusApps.Common.Tools.Uwp;

public class StartupService : IStartupService
{
    /// <inheritdoc/>
    public async Task<StartupState> GetStateAsync(string startupTaskId)
    {
        StartupTask startupTask = await StartupTask.GetAsync(startupTaskId);
        return ToState(startupTask.State);
    }

    /// <inheritdoc/>
    public async Task<bool> TryEnableOnUiThreadAsync(string startupTaskId)
    {
        StartupTask startupTask = await StartupTask.GetAsync(startupTaskId);
        if (startupTask.State is 
            StartupTaskState.DisabledByUser or 
            StartupTaskState.DisabledByUser)
        {
            return false;
        }
        else if (startupTask.State is 
            StartupTaskState.Enabled or
            StartupTaskState.EnabledByPolicy)
        {
            return true;
        }
        else if (startupTask.State is StartupTaskState.Disabled)
        {
            // Disabled but can be enabled
            var newTaskState = await startupTask.RequestEnableAsync();
            return ToState(newTaskState) is StartupState.Enabled;
        }

        return false;
    }

    private StartupState ToState(StartupTaskState taskState)
    {
        return taskState switch
        {
            StartupTaskState.Disabled => StartupState.Disabled,
            StartupTaskState.DisabledByUser => StartupState.DisabledByUser,
            StartupTaskState.Enabled => StartupState.Enabled,
            StartupTaskState.DisabledByPolicy => StartupState.Disallowed,
            StartupTaskState.EnabledByPolicy => StartupState.Enabled,
            _ => StartupState.Disallowed,
        };
    }
}
