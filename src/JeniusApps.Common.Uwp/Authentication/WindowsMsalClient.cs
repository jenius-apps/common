using JeniusApps.Common.Telemetry;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable

namespace JeniusApps.Common.Authentication.Uwp;

public class WindowsMsalClient : IMsalClient
{
    // Ref: https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-client-application-configuration#effective-audience
    public const string ConsumerAuthority = "https://login.microsoftonline.com/consumers";
    public const string CommonAuthority = "https://login.microsoftonline.com/common";

    private readonly ITelemetry _telemetry;
    private readonly string _clientId;
    private readonly IPublicClientApplication _msalSdkClient;

    public event EventHandler<string?>? InteractiveSignInCompleted;

    public WindowsMsalClient(ITelemetry telemetry, string clientId, string authorityUrl)
    {
        _telemetry = telemetry;
        _clientId = clientId;

        _msalSdkClient = PublicClientApplicationBuilder
            .Create(_clientId)
            .WithAuthority(authorityUrl)
            .WithBroker() // See note below.
            .Build();

        // ****** WithBroker notes ******
        //
        // If using WithBroker(), no need for WithRedirectUri().
        // Instead, withBroker works only when we include special redirect URIs in Azure Portal:
        // ms-appx-web://microsoft.aad.brokerplugin/<package SID here>
        //
        // To find your SID, go to partner center > Product Identity. 
    }

    /// <inheritdoc/>
    public async Task<string?> GetTokenSilentAsync(string[] scopes)
    {
        try
        {
            var accounts = await _msalSdkClient.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();
            var authResult = await _msalSdkClient
                .AcquireTokenSilent(scopes, firstAccount)
                .ExecuteAsync();
            return authResult.AccessToken;
        }
        catch (MsalUiRequiredException)
        {
            // this is fine
        }
        catch (MsalException e) when (e.ErrorCode == "user_null")
        {
            // this is fine
        }
        catch (MsalException e)
        {
            _telemetry.TrackError(e, new Dictionary<string, string>
            {
                { "trace", e.StackTrace },
                { "scopes", string.Join(",", scopes) }
            });
        }
        catch (HttpRequestException)
        {
            // no internet
        }

        return "";
    }

    /// <inheritdoc/>
    public async Task RequestInteractiveSignIn(string[] scopes, string[]? extraScopes = null)
    {
        try
        {
            var builder = _msalSdkClient.AcquireTokenInteractive(scopes);

            if (extraScopes is not null)
            {
                builder = builder.WithExtraScopesToConsent(extraScopes);
            }

            var authResult = await builder.ExecuteAsync();
            InteractiveSignInCompleted?.Invoke(this, authResult?.AccessToken);
        }
        catch (MsalException e) when (e.ErrorCode == "authentication_canceled")
        {
            InteractiveSignInCompleted?.Invoke(this, string.Empty);
        }
        catch (MsalException e)
        {
            InteractiveSignInCompleted?.Invoke(this, string.Empty);

            _telemetry.TrackError(e, new Dictionary<string, string>
            {
                { "trace", e.StackTrace },
                { "scopes", string.Join(",", scopes) },
                { "extraScopes", string.Join(",", extraScopes ?? Array.Empty<string>()) }
            });
        }
    }

    /// <inheritdoc/>
    public async Task SignOutAsync()
    {
        var accounts = await _msalSdkClient.GetAccountsAsync();
        foreach (var a in accounts)
        {
            await _msalSdkClient.RemoveAsync(a);
        }
    }
}
