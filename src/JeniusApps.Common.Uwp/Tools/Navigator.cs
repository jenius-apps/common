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
        private INavigator? _innerNavigator;

        /// <inheritdoc/>
        public event EventHandler<string>? PageNavigated;

        public Navigator(IReadOnlyDictionary<string, Type> pageTypeMap)
        {
            _pageTypeMap = pageTypeMap;
        }

        /// <inheritdoc/>
        public void SetFrame(object frame) => _frame = frame;

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
        public void GoBack(PageTransition transition = PageTransition.None, bool innerFrameGoBack = false)
        {
            if (_frame is Frame f && f.CanGoBack)
            {
                f.GoBack(ToTransitionInfo(transition));

                if (innerFrameGoBack && _innerNavigator is { } innerNav)
                {
                    innerNav.GoBack();
                }
            }
        }

        /// <inheritdoc/>
        public void SetInnerNavigator(INavigator inner) => _innerNavigator = inner;

        private NavigationTransitionInfo ToTransitionInfo(PageTransition transition)
        {
            return transition switch
            {
                PageTransition.None => new SuppressNavigationTransitionInfo(),
                PageTransition.Drill => new DrillInNavigationTransitionInfo(),
                PageTransition.SlideFromBottom => new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom },
                PageTransition.SlideFromLeft => new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft },
                PageTransition.SlideFromRight => new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight },
                _ => new SuppressNavigationTransitionInfo()
            };
        }
    }
}
