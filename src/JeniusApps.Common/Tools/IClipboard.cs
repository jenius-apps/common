namespace JeniusApps.Common.Tools
{
    /// <summary>
    /// Interface for clipboard functionality.
    /// </summary>
    public interface IClipboard
    {
        /// <summary>
        /// Copies the given text to the system clipboard.
        /// </summary>
        /// <param name="text">The text to copy.</param>
        /// <returns>True if the copy was successful. False, otherwise.</returns>
        bool CopyToClipboard(string text);
    }
}
