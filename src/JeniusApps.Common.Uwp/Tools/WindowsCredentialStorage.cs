using System;
using System.Diagnostics;
using Windows.Security.Credentials;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp;

public sealed class WindowsCredentialStorage : ISecureCredentialStorage
{
    private readonly string _resourceName;

    public WindowsCredentialStorage(string resourceName)
    {
        _resourceName = resourceName;
    }

    /// <inheritdoc/>
    public bool SetCredential(string key, string credential)
    {
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(credential))
        {
            return false;
        }

        PasswordVault vault = new();
        vault.Add(new PasswordCredential(_resourceName, key, credential));
        return true;
    }

    /// <inheritdoc/>
    public string? GetCredential(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return null;
        }

        try
        {
            PasswordVault vault = new();
            PasswordCredential credential = vault.Retrieve(_resourceName, key);
            credential.RetrievePassword();
            return credential.Password;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return null;
        }
    }
}
