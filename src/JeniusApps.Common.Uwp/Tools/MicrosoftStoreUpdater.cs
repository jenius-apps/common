using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using System.Diagnostics.CodeAnalysis;
using Windows.Services.Store;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp;

/// <summary>
/// Class for updating the app using Microsoft Store SDK.
/// </summary>
public sealed class MicrosoftStoreUpdater : IAppStoreUpdater
{
    private StoreContext? _context;
    private IReadOnlyList<StorePackageUpdate> _updates = [];

    /// <inheritdoc/>
    public event EventHandler<double>? ProgressChanged;

    /// <inheritdoc/>
    public event EventHandler? UpdateAvailable;

    /// <inheritdoc/>
    public async Task<bool> TrySilentDownloadAsync()
    {
        var hasUpdates = await CheckForUpdatesAsync();

        if (!hasUpdates || !_context.CanSilentlyDownloadStorePackageUpdates)
        {
            return false;
        }

        StorePackageUpdateResult downloadResult = await _context.TrySilentDownloadStorePackageUpdatesAsync(_updates);
        return downloadResult.OverallState is StorePackageUpdateState.Completed;
    }

    /// <inheritdoc/>
    public async Task<bool> TrySilentDownloadAndInstallAsync()
    {
        var hasUpdates = await CheckForUpdatesAsync();

        if (!hasUpdates || !_context.CanSilentlyDownloadStorePackageUpdates)
        {
            return false;
        }

        StorePackageUpdateResult downloadResult = await _context.TrySilentDownloadAndInstallStorePackageUpdatesAsync(_updates);
        return downloadResult.OverallState is StorePackageUpdateState.Completed;
    }

    /// <inheritdoc/>
    [MemberNotNull(nameof(_context))]
    public async Task<bool> CheckForUpdatesAsync()
    {
        _context ??= StoreContext.GetDefault();

        try
        {
            _updates = await _context.GetAppAndOptionalStorePackageUpdatesAsync();
        }
        catch (FileNotFoundException)
        {
            // This exception occurs if the app is not associated with the store.
            return false;
        }

        if (_updates is null)
        {
            return false;
        }

        if (_updates.Count > 0)
        {
            UpdateAvailable?.Invoke(this, EventArgs.Empty);
        }

        return _updates.Count > 0;
    }

    /// <inheritdoc/>
    public async Task<bool?> TryApplyUpdatesAsync()
    {
        if (_updates.Count == 0)
        {
            return null;
        }

        _context ??= StoreContext.GetDefault();

        IAsyncOperationWithProgress<StorePackageUpdateResult, StorePackageUpdateStatus> downloadOperation =
                _context.RequestDownloadAndInstallStorePackageUpdatesAsync(_updates);

        downloadOperation.Progress = (asyncInfo, progress) =>
        {
            ProgressChanged?.Invoke(null, progress.PackageDownloadProgress);
        };

        var result = await downloadOperation.AsTask();

        return result.OverallState is StorePackageUpdateState.Completed;
    }
}