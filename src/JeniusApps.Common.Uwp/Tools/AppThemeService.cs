using JeniusApps.Common.Settings;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp;

public sealed class AppThemeService : IAppThemeService
{
    private readonly IUserSettings _userSettings;
    private readonly string _themeSettingsKey;
    private Frame? _frame;

    public event EventHandler<bool>? ActualThemeChanged;

    public AppThemeService(
        IUserSettings userSettings,
        string themeSettingsKey = "themeSetting")
    {
        _userSettings = userSettings;
        _themeSettingsKey = themeSettingsKey;
    }

    /// <inheritdoc/>
    public void SetFrame(object frame)
    {
        if (_frame is Frame oldFrame)
        {
            oldFrame.ActualThemeChanged -= OnFrameThemeChanged;
        }

        if (frame is Frame newFrame)
        {
            _frame = newFrame;
            _frame.ActualThemeChanged += OnFrameThemeChanged;
        }
    }

    /// <inheritdoc/>
    public void ApplyTheme(AppTheme appTheme)
    {
        if (_frame is null)
        {
            return;
        }

        _frame.RequestedTheme = ToElementTheme(appTheme);
        _userSettings.Set(_themeSettingsKey, appTheme.ToString().ToLower());
    }    
    
    /// <inheritdoc/>
    public void ApplyTheme()
    {
        if (_frame is null)
        {
            return;
        }

        _frame.RequestedTheme = ToElementTheme(GetAppTheme());
    }

    /// <inheritdoc/>
    public AppTheme GetAppTheme()
    {
        return ToAppTheme(_userSettings.Get<string>(_themeSettingsKey));
    }

    /// <inheritdoc/>
    public bool IsDark()
    {
        if (_frame is null)
        {
            return false;
        }

        return _frame.ActualTheme is ElementTheme.Dark;
    }

    private void OnFrameThemeChanged(FrameworkElement sender, object args)
    {
        ActualThemeChanged?.Invoke(sender, sender.ActualTheme is ElementTheme.Dark);
    }

    private static ElementTheme ToElementTheme(AppTheme appTheme)
    {
        return appTheme switch
        {
            AppTheme.Dark => ElementTheme.Dark,
            AppTheme.Light => ElementTheme.Light,
            _ => ElementTheme.Default,
        };
    }

    private static AppTheme ToAppTheme(string? themeSetting)
    {
        return (themeSetting?.ToLower()) switch
        {
            "light" => AppTheme.Light,
            "dark" => AppTheme.Dark,
            _ => AppTheme.Default,
        };
    }
}