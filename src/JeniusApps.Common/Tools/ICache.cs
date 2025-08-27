using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JeniusApps.Common.Tools;

/// <summary>
/// Interface for a cache class.
/// </summary>
/// <typeparam name="T">Any type.</typeparam>
public interface ICache<T>
{
    /// <summary>
    /// Retrieves list of items from cache.
    /// </summary>
    /// <param name="token">A cancellation token.</param>
    /// <returns>List of items from cache.</returns>
    public Task<IReadOnlyList<T>> GetItemsAsync(CancellationToken token);

    /// <summary>
    /// Retrieves list of items from cache based on given IDs.
    /// </summary>
    /// <param name="ids">List of item IDs to fetch.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>List of items from cache.</returns>
    public Task<IReadOnlyList<T>> GetItemsAsync(IReadOnlyList<string> ids, CancellationToken token);

    /// <summary>
    /// Retrieves dictionary of items from cache.
    /// </summary>
    /// <remarks>
    /// If the item can't be placed into a dictionary, 
    /// a not supported exception may be thrown because this is
    /// a design flaw in your code, not a bug in the app. Do not call
    /// this if you know the item can't be placed into a dictionary.
    /// </remarks>
    /// <exception cref="NotSupportedException"/>
    /// <param name="token">A cancellation token.</param>
    /// <returns>Dictionary of items from cache.</returns>
    public Task<IReadOnlyDictionary<string, T>> GetItemsAsDictionaryAsync(CancellationToken token);

    /// <summary>
    /// Retrieves dictionary of items from cache based on given IDs.
    /// </summary>
    /// <remarks>
    /// If the item can't be placed into a dictionary, 
    /// a not supported exception may be thrown because this is
    /// a design flaw in your code, not a bug in the app. Do not call
    /// this if you know the item can't be placed into a dictionary.
    /// </remarks>
    /// <param name="ids">List of item IDs to fetch.</param>
    /// <param name="token">A cancellation token.</param>
    /// <exception cref="NotSupportedException"/>
    /// <returns>Dictionary of items from cache.</returns>
    public Task<IReadOnlyDictionary<string, T>> GetItemsAsDictionaryAsync(IReadOnlyList<string> ids, CancellationToken token);

    /// <summary>
    /// Retrieves specific item from cache.
    /// </summary>
    /// <param name="id">The target item's ID.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>The item from cache, or null if not found.</returns>
    Task<T?> GetItemAsync(string id, CancellationToken token);

    /// <summary>
    /// Deletes the given items.
    /// </summary>
    /// <param name="ids">List of item IDs to delete.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>True if operation was successful.</returns>
    Task<bool> DeleteItemsAsync(IReadOnlyList<string> ids, CancellationToken token);

    /// <summary>
    /// Adds the given items to the cache.
    /// </summary>
    /// <param name="items">List of items to add.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>True if operation was successful.</returns>
    Task<bool> AddItemsAsync(IReadOnlyList<T> items, CancellationToken token);

    /// <summary>
    /// Overwrites the cache with the given items.
    /// </summary>
    /// <param name="items">List of items to save.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>True if operation was successful.</returns>
    Task<bool> OverwriteAsync(IReadOnlyList<T> items, CancellationToken token);

    /// <summary>
    /// If the item exists, attempts to replace it with the new item.
    /// </summary>
    /// <param name="newItem">The new item that will overwrite the old item.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>True if the update was successful.</returns>
    Task<bool> UpdateAsync(T newItem, CancellationToken token);

    /// <summary>
    /// Clears the cache and leaves it blank.
    /// Subsequent cache calls will operate
    /// as if the cache was first constructed.
    /// </summary>
    Task ClearCacheAsync();
}

/// <summary>
/// A wrapper class for items that have cache expiration.
/// </summary>
/// <typeparam name="T">Any type.</typeparam>
public class CachedItem<T>
{
    /// <summary>
    /// A time in the future when the item is expired.
    /// </summary>
    public required DateTime ExpirationTime { get; init; }

    /// <summary>
    /// The data that can expire.
    /// </summary>
    public required T Data { get; init; }
}
