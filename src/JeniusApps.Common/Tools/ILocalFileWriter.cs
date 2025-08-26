using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JeniusApps.Common.Tools;

/// <summary>
/// Interface for reading and writing files to the local application folder.
/// </summary>
public interface ILocalFileWriter
{
    /// <summary>
    /// Generic method for writing a file to local directory.
    /// </summary>
    /// <param name="stream">The stream of the file.</param>
    /// <param name="nameWithExt">The name of the file with extension. E.g. Wind.jpg.</param>
    /// <param name="localDirName">Optional. The name of the subdirectory to place the file in. 
    /// <param name="token">Cancellation token.</param>
    /// If null or empty, then the root local directory will be used.</param>
    /// <returns>Full path of the written file.</returns>
    Task<string?> WriteAsync(Stream stream, string nameWithExt, CancellationToken token, string? localDirName = null);

    /// <summary>
    /// Generic method for writing string content to a file in the local directory.
    /// </summary>
    /// <param name="content">The string content to write.</param>
    /// <param name="nameWithExt">The name of the file with extension. E.g. Wind.jpg.</param>
    /// <param name="localDirName">Optional. The name of the subdirectory to place the file in. 
    /// <param name="token">Cancellation token.</param>
    /// If null or empty, then the root local directory will be used.</param>
    /// <returns>Full path of the written file.</returns>
    Task<string?> WriteAsync(string content, string nameWithExt, CancellationToken token, string? localDirName = null);

    /// <summary>
    /// Reads the contents of the specified local file and returns the value.
    /// Returns string.Empty if file not found or path is invalid.
    /// </summary>
    /// <param name="relativeLocalPath">The relative path for a local file to read.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The string content of the file.</returns>
    Task<string> ReadAsync(string relativeLocalPath, CancellationToken token);

    /// <summary>
    /// Deletes the given file.
    /// </summary>
    /// <param name="absolutePathInLocalStorage">The absolute path for a file inside local storage.
    /// If the path is outside of local storage, deletion will likely fail due to file permission restrictions.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>True if successful, false otherwise.</returns>
    Task<bool> DeleteAsync(string absolutePathInLocalStorage, CancellationToken token);

    /// <summary>
    /// Opens the given file for read and returns the stream.
    /// </summary>
    /// <param name="absolutePathInLocalStorage">The absolute path for a file inside local storage.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>A stream if file was successfully opened. Null, otherwise.</returns>
    Task<Stream?> GetStreamAsync(string absolutePathInLocalStorage, CancellationToken token);
}
