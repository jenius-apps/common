using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp;

public class LocalFileWriter : ILocalFileWriter
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new();

    /// <inheritdoc/>
    public async Task<Stream?> GetStreamAsync(string absolutePathInLocalStorage, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        StorageFile file = await StorageFile.GetFileFromPathAsync(absolutePathInLocalStorage);
        return file is null ? null : await file.OpenStreamForReadAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(string absolutePathInLocalStorage, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(absolutePathInLocalStorage))
        {
            return false;
        }

        try
        {
            var file = await StorageFile.GetFileFromPathAsync(absolutePathInLocalStorage);
            token.ThrowIfCancellationRequested();
            await file.DeleteAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<string> ReadAsync(string relativeLocalPath, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(relativeLocalPath))
        {
            return string.Empty;
        }

        var semaphore = _semaphores.GetOrAdd(relativeLocalPath, new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync();

        try
        {
            IStorageItem targetLocation = await ApplicationData.Current.LocalFolder.TryGetItemAsync(relativeLocalPath);
            token.ThrowIfCancellationRequested();

            if (targetLocation is StorageFile file)
            {
                return await FileIO.ReadTextAsync(file);
            }
        }
        finally
        {
            semaphore.Release();
        }

        return string.Empty;
    }

    /// <inheritdoc/>
    public async Task<string?> WriteAsync(Stream stream, string nameWithExt, CancellationToken token, string? localDirName = null)
    {
        token.ThrowIfCancellationRequested();

        string relativeLocalPath = localDirName is null
            ? nameWithExt
            : $"{localDirName}/{nameWithExt}";

        if (string.IsNullOrEmpty(relativeLocalPath))
        {
            return null;
        }

        var semaphore = _semaphores.GetOrAdd(relativeLocalPath, new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync();
        try
        {
            StorageFile targetLocation = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                relativeLocalPath,
                CreationCollisionOption.ReplaceExisting);

            token.ThrowIfCancellationRequested();

            if (targetLocation is not null)
            {
                using IRandomAccessStream fileStream = await targetLocation.OpenAsync(FileAccessMode.ReadWrite);
                await stream.CopyToAsync(fileStream.AsStreamForWrite());
                await fileStream.FlushAsync();

                return targetLocation.Path;
            }
        }
        finally
        {
            semaphore.Release();
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<string?> WriteAsync(string content, string nameWithExt, CancellationToken token, string? localDirName = null)
    {
        token.ThrowIfCancellationRequested();

        string relativeLocalPath = localDirName is null
            ? nameWithExt
            : $"{localDirName}/{nameWithExt}";

        if (string.IsNullOrEmpty(relativeLocalPath))
        {
            return null;
        }

        var semaphore = _semaphores.GetOrAdd(relativeLocalPath, new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync();
        try
        {
            StorageFile targetLocation = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                relativeLocalPath,
                CreationCollisionOption.ReplaceExisting);

            token.ThrowIfCancellationRequested();

            if (targetLocation is not null)
            {
                await FileIO.WriteTextAsync(targetLocation, content);
                return targetLocation.Path;
            }
        }
        finally
        {
            semaphore.Release();
        }

        return null;
    }
}
