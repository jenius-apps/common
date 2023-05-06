using System;

namespace JeniusApps.Common.Tools
{
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
        void InitializeFrame(object frame);

        /// <summary>
        /// Navigates the frame to the given page.
        /// </summary>
        /// <param name="pageKey">The page to naviate to.</param>
        /// <param name="navArgs">Optional. An object that will be passed to the destination page.</param>
        void NavigateTo(string pageKey, object? navArgs = null);

        /// <summary>
        /// Safely goes back one level in the frame's navigation stack.
        /// </summary>
        /// <param name="transition">Optional. The transition to use when navigating back.</param>
        void GoBack(PageTransition transition = PageTransition.None);
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
        /// Represents a slide left or right transition animation.
        /// </summary>
        Slide
    }
}
