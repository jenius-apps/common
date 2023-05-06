using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp
{
    public class NavigatorFactory : INavigatorFactory
    {
        private readonly Dictionary<string, INavigator> _navigators = new();

        /// <inheritdoc/>
        public INavigator Create(
            string navigatorName, 
            object? constructorParameter, 
            object? frame)
        {
            if (_navigators.TryGetValue(navigatorName, out INavigator navigator))
            {
                return navigator;
            }

            if (constructorParameter is IReadOnlyDictionary<string, Type> dictionary &&
                frame is Frame f)
            {
                var newNavigator = new Navigator(dictionary);
                newNavigator.InitializeFrame(f);
                _navigators.TryAdd(navigatorName, newNavigator);
                return newNavigator;
            }
            else
            {
                throw new ArgumentException("The constructor parameter or the frame object provided are invalid.");
            }
        }

        /// <inheritdoc/>
        public INavigator? Get(string navigatorName)
        {
            if (_navigators.TryGetValue(navigatorName, out INavigator navigator))
            {
                return navigator;
            }

            return null;
        }
    }
}
