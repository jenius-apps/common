using Microsoft.Toolkit.Uwp.Helpers;
using System;
using Windows.Storage;
using Windows.System.Power;
using Windows.System.Profile;
using Windows.UI.ViewManagement;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp;

public class SystemInfoProvider : ISystemInfoProvider
{
    private bool? _isWin11;

    /// <inheritdoc/>
    public DateTime FirstUseDate()
    {
        return SystemInformation.Instance.FirstUseTime;
    }

    /// <inheritdoc/>
    public string GetCulture()
    {
        return SystemInformation.Instance.Culture.Name;
    }

    /// <inheritdoc/>
    public bool IsCompact()
    {
        return ApplicationView.GetForCurrentView().ViewMode == ApplicationViewMode.CompactOverlay;
    }

    /// <inheritdoc/>
    public string GetDeviceFamily()
    {
        return AnalyticsInfo.VersionInfo.DeviceFamily;
    }

    /// <inheritdoc/>
    public bool IsFirstRun()
    {
        return SystemInformation.Instance.IsFirstRun;
    }

    /// <inheritdoc/>
    public bool IsWin11()
    {
        _isWin11 ??= Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent(
            "Windows.Foundation.UniversalApiContract",
            14);

        return _isWin11.Value;
    }

    /// <inheritdoc/>
    public string LocalFolderPath()
    {
        return ApplicationData.Current.LocalFolder.Path;
    }

    /// <inheritdoc/>
    public bool IsOnBatterySaver()
    {
        return PowerManager.EnergySaverStatus == EnergySaverStatus.On;
    }
}
