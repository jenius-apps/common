using System;
using Windows.System;

namespace JeniusApps.Common.Tools.Uwp
{
    public class WindowsDispatcherQueue : IDispatcherQueue
    {
        private readonly DispatcherQueue _dispatcherQueue;

        public WindowsDispatcherQueue()
        {
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        }

        /// <inheritdoc/>
        public void TryEnqueue(Action action)
        {
            _dispatcherQueue.TryEnqueue(() => action());
        }
    }
}
