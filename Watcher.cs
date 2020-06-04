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
        /// <param name="saveDataToTempStorage">By setting this to true, the lib will save it's daily date 
        /// to a temp storage so it can keep data in case of early shutdown</param>
        public Watcher(Resolution chosenResolution, bool saveDataToTempStorage = false)
        {
            UsageStore store = new UsageStore(chosenResolution, saveDataToTempStorage);
            uService = new UsageService(store);
        }

        /// <summary>
        /// Gives back the time since recording began
        /// </summary>
        /// <returns></returns>
        public TimeSpan UsageSoFar()
        {
            return uService.UsageSoFar();
        }

        /// <summary>
        /// Gives back the usage time since the last sync time, or if this is the first sync, than
        /// since startup
        /// </summary>
        /// <returns></returns>
        public TimeSpan UsageSinceLastAccess()
        {
            return uService.UsageSinceLastSync();
        }

        /// <summary>
        /// Gives back the time inbetween the given times
        /// </summary>
        /// <returns></returns>
        public TimeSpan UsageForGivenTimeframe(DateTime startTime, DateTime endTime)
        {
            return uService.UsageForTime(startTime, endTime);
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
