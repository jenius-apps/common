using Windows.ApplicationModel.DataTransfer;

namespace JeniusApps.Common.Tools.Uwp
{
    internal class WindowsClipboard : IClipboard
    {
        /// <inheritdoc/>
        public bool CopyToClipboard(string text)
        {
            try
            {
                DataPackage dataPackage = new()
                {
                    RequestedOperation = DataPackageOperation.Copy
                };

                dataPackage.SetText(text);
                Clipboard.SetContent(dataPackage);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
