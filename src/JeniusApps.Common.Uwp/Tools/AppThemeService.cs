using JeniusApps.Common.Settings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp;

public sealed class AppThemeService : IAppThemeService
{
    private readonly IUserSettings _userSettings;
    private readonly string _themeSettingsKey;
    private Frame? _frame;

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
        if (frame is Frame f)
        {
            _frame = f;
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