using System;

namespace JeniusApps.Common.Tools;

/// <summary>
/// Interface that aids with page navigation.
/// </summary>
public interface INavigator
{
    /// <summary>
    /// Raised when a navigation was performed successfully.
    /// The string payload is the pageKey.
    /// </summary>
    event EventHandler<string>? PageNavigated;

    /// <summary>
    /// Initializes the frame that will be used for navigation.
    /// </summary>
    /// <param name="frame">The frame to navigate.</param>
    void SetFrame(object frame);

    /// <summary>
    /// Sets an optional inner navigator that can be manipulated.
    /// </summary>
    /// <param name="inner">The navigator representing an inner content frame.</param>
    void SetInnerNavigator(INavigator inner);

    /// <summary>
    /// Navigates the frame to the given page.
    /// </summary>
    /// <param name="pageKey">The page to naviate to.</param>
    /// <param name="navArgs">Optional. An object that will be passed to the destination page.</param>
    /// <param name="transition">Optional. Specifies the transition animation to use.</param>
    void NavigateTo(
        string pageKey,
        object? navArgs = null,
        PageTransition transition = PageTransition.None);

    /// <summary>
    /// Safely goes back one level in the frame's navigation stack.
    /// </summary>
    /// <param name="transition">Optional. The transition to use when navigating back.</param>
    /// <param name="innerFrameGoBack">Optional. If true, a GoBack command will be executed on the inner frame as well.</param>
    void GoBack(
        PageTransition transition = PageTransition.None,
        bool innerFrameGoBack = false);

    /// <summary>
    /// Safely goes forward in the frame's navigation stack.
    /// </summary>
    void GoForward();

    /// <summary>
    /// Retrieves the key for the current page.
    /// </summary>
    string GetCurrentPageKey();
}

/// <summary>
/// Enums that represent supported page transitions.
/// </summary>
public enum PageTransition
{
    /// <summary>
    /// Represents no page transition animation.
    /// </summary>
    None,

    /// <summary>
    /// Represents a drill in or out transition animation.
    /// </summary>
    Drill,

    /// <summary>
    /// Represents a slide from bottom transition animation.
    /// </summary>
    SlideFromBottom,

    /// <summary>
    /// Represents a slide from left transition animation.
    /// </summary>
    SlideFromLeft,

    /// <summary>
    /// Represents a slide from right transition animation.
    /// </summary>
    SlideFromRight,
}
