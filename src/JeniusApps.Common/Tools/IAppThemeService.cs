namespace JeniusApps.Common.Tools;

/// <summary>
/// Interface for managing the app's theme.
/// </summary>
public interface IAppThemeService
{
    /// <summary>
    /// Sets the frame of the app whose theme can be changed.
    /// </summary>
    /// <param name="frame">The frame object.</param>
    void SetFrame(object frame);

    /// <summary>
    /// Applies the theme to the frame and updates settings.
    /// </summary>
    /// <param name="appTheme">The theme to apply.</param>
    void ApplyTheme(AppTheme appTheme);

    /// <summary>
    /// Applies the theme already stored in settings.
    /// </summary>
    void ApplyTheme();

    /// <summary>
    /// Retrieves the theme from settings.
    /// </summary>
    /// <returns>The app theme from settings.</returns>
    AppTheme GetAppTheme();
}

/// <summary>
/// An enum represnting possible app themes.
/// </summary>
public enum AppTheme
{
    /// <summary>
    /// System default theme. This lets the app adapt to
    /// whatever the user has set as the theme for the OS.
    /// </summary>
    Default,

    /// <summary>
    /// Forces the app to use light theme.
    /// </summary>
    Light,

    /// <summary>
    /// Forces the app to use dark theme.
    /// </summary>
    Dark,
}
