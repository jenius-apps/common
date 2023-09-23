using System;
using System.Threading.Tasks;

namespace JeniusApps.Common.Authentication;

/// <summary>
/// Interface for signing in to a
/// Microsoft account and retrieving a Microsoft Graph token.
/// </summary>
public interface IMsalClient
{
    /// <summary>
    /// Fires when sign in process completes.
    /// String is the access token if successful. Null or empty
    /// otherwise.
    /// </summary>
    event EventHandler<string?>? InteractiveSignInCompleted;

    /// <summary>
    /// Attempts to sign in silently and retrieve a token
    /// for the given scopes.
    /// Returns null if silent auth was unsuccessful.
    /// </summary>
    /// <returns>A token if sign in was successful, and null if not.</returns>
    Task<string?> GetTokenSilentAsync(string[] scopes);

    /// <summary>
    /// Attempts to sign in and retrieve at token. User will be prompted.
    /// Result will be communicated via <see cref="InteractiveSignInCompleted"/>.
    /// </summary>
    Task RequestInteractiveSignIn(string[] scopes, string[]? extraScopes = null);

    /// <summary>
    /// Signs out the user.
    /// </summary>
    Task SignOutAsync();
}
