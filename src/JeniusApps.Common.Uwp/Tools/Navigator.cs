using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp
{
    public class Navigator : INavigator
    {
        private readonly IReadOnlyDictionary<string, Type> _pageTypeMap;
        private object? _frame;

        /// <inheritdoc/>
        public event EventHandler<string>? PageNavigated;

        public Navigator(IReadOnlyDictionary<string, Type> pageTypeMap)
        {
            _pageTypeMap = pageTypeMap;
        }

        /// <inheritdoc/>
        public void InitializeFrame(object frame) => _frame ??= frame;

        /// <inheritdoc/>
        public void NavigateTo(string pageKey, object? navArgs = null, PageTransition transition = PageTransition.None)
        {
            if (!_pageTypeMap.TryGetValue(pageKey, out Type pageType))
            {
                return;
            }

            if (_frame is Frame f)
            {
                f.Navigate(pageType, navArgs, ToTransitionInfo(transition));
                PageNavigated?.Invoke(this, pageKey);
            }
        }

        /// <inheritdoc/>
        public void GoBack(PageTransition transition = PageTransition.None)
        {
            if (_frame is Frame f && f.CanGoBack)
            {
                f.GoBack(ToTransitionInfo(transition));
            }
        }

        private NavigationTransitionInfo ToTransitionInfo(PageTransition transition)
        {
            return transition switch
            {
                PageTransition.None => new SuppressNavigationTransitionInfo(),
                PageTransition.Drill => new DrillInNavigationTransitionInfo(),
                PageTransition.Slide => new SlideNavigationTransitionInfo(),
                _ => new SuppressNavigationTransitionInfo()
            };
        }
    }
}
