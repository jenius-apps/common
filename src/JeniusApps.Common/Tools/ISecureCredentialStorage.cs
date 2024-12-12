namespace JeniusApps.Common.Tools;

/// <summary>
/// An interface for securely storing credentials.
/// </summary>
public interface ISecureCredentialStorage
{
    /// <summary>
    /// Stores the given key and the credential.
    /// </summary>
    /// <param name="key">The credential's key.</param>
    /// <param name="credential">The credential.</param>
    /// <returns>True if successful, false otherwise.</returns>
    public bool SetCredential(string key, string credential);

    /// <summary>
    /// Retrieves the credential based on the given key.
    /// </summary>
    /// <param name="key">The credential's key.</param>
    /// <returns>The credential or null.</returns>
    public string? GetCredential(string key);
}
