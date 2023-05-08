using System;
using System.Threading.Tasks;

namespace JeniusApps.Common.Tools
{
    /// <summary>
    /// Interface for launching URIs.
    /// </summary>
    public interface IUriLauncher
    {
        /// <summary>
        /// Launches the give URI.
        /// </summary>
        /// <param name="uri">The URI to launch.</param>
        /// <returns>True if launch was successful. False, otherwise.</returns>
        Task<bool> LaunchUriAsync(Uri uri);
    }
}
