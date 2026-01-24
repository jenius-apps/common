using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Services.Store;

#nullable enable

namespace JeniusApps.Common.Store.Uwp;

public class StoreService : IIapService
{
    private readonly IReadOnlyList<string> _subscriptionPrefixes;
    private readonly IReadOnlyList<string> _lifetimeExactIapIds;
    private readonly ConcurrentDictionary<string, (int Version, StoreProduct Product)> _versionedProductsCache = new();
    private readonly ConcurrentDictionary<string, StoreProduct> _productsCache = new();
    private readonly ConcurrentDictionary<string, bool> _ownershipCache = new();
    private readonly SemaphoreSlim _versionedProductsLock = new(1, 1);
    private readonly bool _debugAllOwned;
    private StoreContext? _context;

    /// <inheritdoc/>
    public event EventHandler<string>? ProductPurchased;

    /// <param name="subscriptionPrefixes">List of IAP subscription prefixes.</param>
    /// <param name="lifetimeExactIapIds">List of exact IDs for for lifetime IAPs.</param>
    /// <param name="debugAllOwned">USE WITH CAUTION. DESIGNED FOR DEBUG SCENARIOS ONLY. If true, all ownership checks return as true.</param>
    public StoreService(
        IReadOnlyList<string> subscriptionPrefixes,
        IReadOnlyList<string> lifetimeExactIapIds,
        bool debugAllOwned = false)
    {
        _subscriptionPrefixes = subscriptionPrefixes;
        _lifetimeExactIapIds = lifetimeExactIapIds;
        _debugAllOwned = debugAllOwned;
    }

    /// <inheritdoc/>
    public virtual async Task<bool> IsOwnedAsync(string iapIdToCheck)
    {
        if (_debugAllOwned)
        {
            return true;
        }

        if (_ownershipCache.TryGetValue(iapIdToCheck, out bool isOwned))
        {
            return isOwned;
        }

        _context ??= StoreContext.GetDefault();

        StoreAppLicense appLicense = await _context.GetAppLicenseAsync();
        if (appLicense is null)
        {
            return false;
        }

        bool isIapSubscription = ContainsSubscriptionPrefix(iapIdToCheck);

        foreach (var addOnLicense in appLicense.AddOnLicenses)
        {
            StoreLicense license = addOnLicense.Value;
            if (!license.IsActive)
            {
                continue;
            }

            if (license.InAppOfferToken == iapIdToCheck || // Handle exact match
                (isIapSubscription && ContainsSubscriptionPrefix(license.InAppOfferToken))) // Handle prefix match such as for subs
            {
                _ownershipCache.TryAdd(iapIdToCheck, true);
                return true;
            }
        }

        _ownershipCache.TryAdd(iapIdToCheck, false);
        return false;
    }

    /// <inheritdoc/>
    public virtual async Task<bool> IsSubscriptionOwnedAsync()
    {
        return await IsAnyOwnedAsync(_subscriptionPrefixes).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> CanShowPremiumButtonsAsync()
    {
        return !await IsAnyOwnedAsync([.. _subscriptionPrefixes, .. _lifetimeExactIapIds]).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> IsAnyOwnedAsync(IReadOnlyList<string> iapIds)
    {
        foreach (var id in iapIds)
        {
            var owned = await IsOwnedAsync(id).ConfigureAwait(false);
            if (owned)
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public virtual async Task<PriceInfo> GetPriceAsync(string iapId, bool isSubscriptionIdFormat = false)
    {
        (string idOnly, _) = SplitIdAndVersion(iapId);
        var addon = isSubscriptionIdFormat
            ? await GetLatestAddonAsync(idOnly).ConfigureAwait(false)
            : await GetAddOnAsync(iapId).ConfigureAwait(false);

        if (addon?.Price is null)
        {
            return new PriceInfo { FormattedPrice = "-" };
        }

        var sku = addon.Skus?.FirstOrDefault();
        bool isSub = sku?.IsSubscription ?? false;

        return new PriceInfo
        {
            IsOnSale = addon.Price.IsOnSale,
            SaleEndDateUtc = addon.Price.SaleEndDate.UtcDateTime,
            FormattedBasePrice = addon.Price.FormattedBasePrice,
            FormattedPrice = isSub ? addon.Price.FormattedRecurrencePrice : addon.Price.FormattedPrice,
            IsSubscription = isSub,
            RecurrenceLength = (int)(sku?.SubscriptionInfo?.BillingPeriod ?? 0),
            RecurrenceUnit = ToDurationUnit(sku?.SubscriptionInfo?.BillingPeriodUnit),
            HasSubTrial = sku?.SubscriptionInfo?.HasTrialPeriod ?? false,
            SubTrialLength = (int)(sku?.SubscriptionInfo?.TrialPeriod ?? 0),
            SubTrialLengthUnit = ToDurationUnit(sku?.SubscriptionInfo?.TrialPeriodUnit),
        };
    }

    private DurationUnit ToDurationUnit(StoreDurationUnit? storeDurationUnit)
    {
        return storeDurationUnit switch
        {
            StoreDurationUnit.Minute => DurationUnit.Minute,
            StoreDurationUnit.Hour => DurationUnit.Hour,
            StoreDurationUnit.Day => DurationUnit.Day,
            StoreDurationUnit.Week => DurationUnit.Week,
            StoreDurationUnit.Month => DurationUnit.Month,
            StoreDurationUnit.Year => DurationUnit.Year,
            _ => DurationUnit.Minute
        };
    }

    private async Task<StoreProduct?> GetLatestAddonAsync(string idOnly)
    {
        if (_versionedProductsCache.TryGetValue(idOnly, out var cachedResult))
        {
            return cachedResult.Product;
        }

        if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
        {
            return null;
        }

        // At this point, the product cache is likely not populated,
        // so obtain the lock and then run the populate method.
        await _versionedProductsLock.WaitAsync().ConfigureAwait(false);

        // Check cache again in case it changed while waiting.
        if (_versionedProductsCache.TryGetValue(idOnly, out cachedResult))
        {
            return cachedResult.Product;
        }

        // Populate the cache.
        await PopulateAddonCacheAsync().ConfigureAwait(false);

        _versionedProductsLock.Release();

        // Try to return the desired add on.
        return _versionedProductsCache.TryGetValue(idOnly, out cachedResult)
            ? cachedResult.Product
            : null;
    }

    private async Task PopulateAddonCacheAsync()
    {
        _context ??= StoreContext.GetDefault();

        // Get all add-ons for this app.
        var result = await _context.GetAssociatedStoreProductsAsync(["Durable", "Consumable"]);
        if (result.ExtendedError is not null)
        {
            return;
        }

        // Find all addons and cache the latest version
        foreach (var item in result.Products)
        {
            StoreProduct product = item.Value;

            (string id, int newVersion) = SplitIdAndVersion(product.InAppOfferToken);
            if (_versionedProductsCache.TryGetValue(id, out var cachedResult) && newVersion > cachedResult.Version)
            {
                _versionedProductsCache[id] = (newVersion, product);
            }
            else
            {
                _versionedProductsCache.TryAdd(id, (newVersion, product));
            }
        }
    }

    /// <inheritdoc/>
    public virtual async Task<bool> BuyAsync(string iapId, bool isSubscriptionIdFormat = false, string? iapIdCacheOverride = null)
    {
        StorePurchaseStatus result = await PurchaseAddOn(iapId, isSubscriptionIdFormat).ConfigureAwait(false);

        if (result == StorePurchaseStatus.Succeeded || result == StorePurchaseStatus.AlreadyPurchased)
        {
            _ownershipCache[iapIdCacheOverride ?? iapId] = true;
            ProductPurchased?.Invoke(this, iapIdCacheOverride ?? iapId);
        }

        return result switch
        {
            StorePurchaseStatus.Succeeded => true,
            StorePurchaseStatus.AlreadyPurchased => true,
            _ => false
        };
    }

    private async Task<StorePurchaseStatus> PurchaseAddOn(string id, bool isSubscriptionIdFormat = false)
    {
        if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
        {
            return StorePurchaseStatus.NetworkError;
        }

        (string idOnly, _) = SplitIdAndVersion(id);

        var addOnProduct = isSubscriptionIdFormat
            ? await GetLatestAddonAsync(idOnly).ConfigureAwait(false)
            : await GetAddOnAsync(id).ConfigureAwait(false);

        if (addOnProduct is null)
            return StorePurchaseStatus.ServerError;

        /// Attempt purchase
        var result = await addOnProduct.RequestPurchaseAsync();
        if (result is null)
            return StorePurchaseStatus.ServerError;

        return result.Status;
    }

    private async Task<StoreProduct?> GetAddOnAsync(string id)
    {
        if (_productsCache.ContainsKey(id))
        {
            return _productsCache[id];
        }

        if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
        {
            return null;
        }

        if (_context is null)
            _context = StoreContext.GetDefault();

        /// Get all add-ons for this app.
        var result = await _context.GetAssociatedStoreProductsAsync(new string[] { "Durable", "Consumable" });
        if (result.ExtendedError is not null)
        {
            return null;
        }

        foreach (var item in result.Products)
        {
            StoreProduct product = item.Value;

            if (product.InAppOfferToken == id)
            {
                // gets add-on
                _productsCache.TryAdd(id, product);
                return product;
            }
        }

        return null;
    }

    public virtual bool ContainsSubscriptionPrefix(IReadOnlyList<string> ids)
    {
        foreach (var id in ids)
        {
            if (ContainsSubscriptionPrefix(id))
            {
                return true;
            }
        }

        return false;
    }

    public virtual bool ContainsSubscriptionPrefix(string id)
    {
        return _subscriptionPrefixes.Any(x => id.StartsWith(x, StringComparison.OrdinalIgnoreCase));
    }

    private (string, int) SplitIdAndVersion(string iapId)
    {
        if (string.IsNullOrEmpty(iapId))
        {
            return (string.Empty, 0);
        }

        if (iapId.Split('_') is [string id, string version, ..])
        {
            return int.TryParse(version, out int result)
                ? (id, result)
                : (id, 0);
        }

        return (iapId, 0);
    }
}
