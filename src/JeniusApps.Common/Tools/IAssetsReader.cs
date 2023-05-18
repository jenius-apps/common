using System.Threading.Tasks;

namespace JeniusApps.Common.Tools
{
    /// <summary>
    /// Reads data from the installed package location.
    /// </summary>
    public interface IAssetsReader
    {
        /// <summary>
        /// Reads content from file at given relative path.
        /// </summary>
        /// <param name="relativePath">
        /// The case sensitive relative path of the file to read in the installed package location.
        /// E.g. 'Assets/data.json'
        /// </param>
        /// <returns>The string content of the given file.</returns>
        Task<string> ReadFileAsync(string relativePath);
    }
}
