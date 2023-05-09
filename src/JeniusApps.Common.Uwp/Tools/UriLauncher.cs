using System;
using System.Threading.Tasks;
using Windows.System;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp
{
    public class UriLauncher : IUriLauncher
    {
        /// <inheritdoc/>
        public async Task<bool> LaunchUriAsync(Uri uri)
        {
            return await Launcher.LaunchUriAsync(uri);
        }
    }
}
