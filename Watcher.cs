using System;
using UsageWatcher.Service;
using UsageWatcher.Storage;

namespace UsageWatcher
{
    /// <summary>
    /// Watches the system for mouse and keyboard movements
    /// and provides you with simple booleans about system usage
    /// </summary>
    public class Watcher : IDisposable
    {
        private UsageService uService;

        /// <summary>
        /// You must pass the smallest timeframe the software will watch for.
        /// Eg.: If set to TEN_MINUTES, then one mouse movement or keypress in that timeframe
        /// will be considered an active time
        /// </summary>
        /// <param name="resolution">The smallest timeframe the software will watch for</param>
        public Watcher(Resolution chosenResolution)
        {
            UsageStore store = new UsageStore(chosenResolution);
            uService = new UsageService(store);
        }

        public TimeSpan UsageSoFar()
        {
            return uService.UsageSoFar();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                uService.Dispose();
                disposedValue = true;
            }
        }

        ~Watcher()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
