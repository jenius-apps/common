namespace JeniusApps.Common.Tools;

/// <summary>
/// Extension methods for <see cref="ISystemInfoProvider"/>.
/// </summary>
public static class SystemInfoExtensions
{
    /// <summary>
    /// Returns true if the system is desktop.
    /// </summary>
    /// <param name="systemInfoProvider">The <see cref="ISystemInfoProvider"/> to use.</param>
    /// <returns>True if the system is desktop.</returns>
    public static bool IsDesktop(this ISystemInfoProvider systemInfoProvider) => systemInfoProvider.GetDeviceFamily() == "Windows.Desktop";

    /// <summary>
    /// Returns true if the system is Xbox.
    /// </summary>
    /// <param name="systemInfoProvider">The <see cref="ISystemInfoProvider"/> to use.</param>
    /// <returns>True if the system is Xbox.</returns>
    public static bool IsXbox(this ISystemInfoProvider systemInfoProvider) => systemInfoProvider.GetDeviceFamily() == "Windows.Xbox";
}