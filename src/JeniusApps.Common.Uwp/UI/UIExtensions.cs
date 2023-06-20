using Windows.UI.Xaml;

#nullable enable

namespace JeniusApps.Common.UI.Uwp
{
    /// <summary>
    /// Extension methods designed to be used with x:Bind.
    /// </summary>
    public static class UIExtensions
    {
        /// <summary>
        /// Inverts the value.
        /// </summary>
        public static bool Not(this bool value) => !value;

        /// <summary>
        /// Inverts the value and converts the result to visibility.
        /// </summary>
        public static Visibility InvertBoolToVis(this bool value)
        {
            return value
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        /// <summary>
        /// Returns visible if string is null or empty. 
        /// Collapsed, otherwise.
        /// </summary>
        public static Visibility VisibleIfEmpty(this string s)
        {
            return string.IsNullOrEmpty(s)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        /// <summary>
        /// Returns collapsed if string is null or empty. 
        /// Visible, otherwise.
        /// </summary>
        public static Visibility CollapsedIfEmpty(this string s)
        {
            return string.IsNullOrEmpty(s)
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        /// <summary>
        /// Returns collapsed if object is null. 
        /// Visible, otherwise.
        /// </summary>
        public static Visibility CollapsedIfNull(this object obj)
        {
            return obj is null
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        /// <summary>
        /// Returns visible if object is null. 
        /// Collapsed, otherwise.
        /// </summary>
        public static Visibility VisibleIfNull(this object obj)
        {
            return obj is null
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public static Visibility VisibleIfAll(this bool a, bool b)
        {
            return a && b
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public static Visibility VisibleIfAny(this bool a, bool b)
        {
            return a || b
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public static Visibility CollapsedIfAll(this bool a, bool b)
        {
            return a && b
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public static Visibility CollapsedIfAny(this bool a, bool b)
        {
            return a || b
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
    }
}
