using Windows.ApplicationModel.Resources;

namespace JeniusApps.Common.Tools.Uwp
{
    public class ReswLocalizer : ILocalizer
    {
        private readonly ResourceLoader _resourceLoader;

        public ReswLocalizer()
        {
            _resourceLoader = ResourceLoader.GetForCurrentView();
        }

        /// <inheritdoc/>
        public string GetString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            return _resourceLoader.GetString(key);
        }

        /// <inheritdoc/>
        public string GetString(string key, string formatParam)
        {
            return string.Format(_resourceLoader.GetString(key), formatParam);
        }
    }
}
