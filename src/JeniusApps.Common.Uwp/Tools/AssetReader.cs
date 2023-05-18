using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp
{
    public class AssetReader : IAssetsReader
    {
        /// <inheritdoc/>
        public async Task<string> ReadFileAsync(string relativePath)
        {
            StorageFolder currentFolder = Package.Current.InstalledLocation;
            var splits = relativePath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            StorageFile? file = null;

            for (int i = 0; i < splits.Length; i++)
            {
                if (splits.Length - 1 == i)
                {
                    file = await currentFolder.GetFileAsync(splits[i]);
                    break;
                }

                currentFolder = await currentFolder.GetFolderAsync(splits[i]);
            }

            return await FileIO.ReadTextAsync(file);
        }
    }
}
