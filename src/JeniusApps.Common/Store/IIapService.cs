using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeniusApps.Common.Store;

/// <summary>
/// Interface for retrieving IAP ownership info
/// or purchasing IAP.
/// </summary>
public interface IIapService
{
    /// <summary>
    /// Raised when a product was purchased.
    /// The payload is the IAP ID or prefix.
    /// </summary>
    event EventHandler<string>? ProductPurchased;

    /// <summary>
    /// Attempts to buy the IAP item.
    /// </summary>
    /// <param name="iapId">The IAP ID of the add-on we want to purchase.</param>
    /// <param name="latest">
    /// We recommend using <see langword="true"/> when trying to buy a subscription,
    /// so that the latest version is purchased.
    /// </param>
    /// <param name="iapIdCacheOverride">
    /// Optional. The ID to be used when storing the purchase successful result in cache.
    /// Ideal when you have multiple different IAP IDs that point to the same subscription, such as with promo codes.
    /// </param>
    /// <returns>True if the item was purchased successfully.</returns>
    Task<bool> BuyAsync(string iapId, bool latest = false, string? iapIdCacheOverride = null);

    /// <summary>
    /// Determines if premium buttons can be displayed.
    /// </summary>
    Task<bool> CanShowPremiumButtonsAsync();

    /// <summary>
    /// Determines if any of the given list of IDs match with any known subscription prefixes.
    /// </summary>
    /// <param name="ids">The IAP IDs to check.</param>
    /// <returns>True if any IDs match with any known subscription prefixes.</returns>
    bool ContainsSubscriptionPrefix(IReadOnlyList<string> ids);

    /// <summary>
    /// Determines if the given ID match with any known subscription prefixes.
    /// </summary>
    /// <param name="id">The IAP ID to check.</param>
    /// <returns>True if the ID matches with any known subscription prefixes.</returns>
    bool ContainsSubscriptionPrefix(string id);

    /// <summary>
    /// Retrieves the latest price of the item based on the prefix of the IAP ID.
    /// </summary>
    /// <param name="iapId">An IAP ID whose price we want to check.</param>
    /// <returns>A <see cref="PriceInfo"/> object that contains price data.</returns>
    Task<PriceInfo> GetLatestPriceAsync(string iapId);

    /// <summary>
    /// Retrieves the price of the item, using the whole ID.
    /// </summary>
    /// <param name="iapId">The exact IAP ID whose price we want to check</param>
    /// <returns>A <see cref="PriceInfo"/> object that contains price data.</returns>
    Task<PriceInfo> GetPriceAsync(string iapId);

    /// <summary>
    /// Returns true if any of the given in-app purchase IDs
    /// are owned.
    /// </summary>
    Task<bool> IsAnyOwnedAsync(IReadOnlyList<string> iapIds);

    /// <summary>
    /// Check if the ID is already owned.
    /// </summary>
    Task<bool> IsOwnedAsync(string iapIdToCheck);

    /// <summary>
    /// Returns true if the user has an active subscription.
    /// </summary>
    /// <remarks>
    /// Only looks at subsription IAP, not lifetime durable ownerships.
    /// </remarks>
    Task<bool> IsSubscriptionOwnedAsync();
}
