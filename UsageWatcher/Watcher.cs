using System;
using System.Collections.Generic;
using UsageWatcher.Models;
using UsageWatcher.Service;
using UsageWatcher.Storage;
using UsageWatcher.Enums;

namespace UsageWatcher
{
    /// <summary>
    /// Watches the system for mouse and keyboard movements
    /// and provides you with simple booleans about system usage
    /// </summary>
    public class Watcher : IWatcher, IDisposable
    {
        private readonly WatcherService wService;

        /// <summary>
        /// You must pass the smallest timeframe the software will watch for.
        /// Eg.: If set to TEN_MINUTES, then one mouse movement or keypress in that timeframe
        /// will be considered an active time.
        /// DataPrecision is not implemented fully, only High precision works
        /// </summary>
        /// <param name="appName">If saving is enabled the resulting file will have this as prefix</param>
        /// <param name="resolution">The smallest timeframe the software will watch for</param>
        /// <param name="savePreference">Dictates how usage data will be saved/stored</param>
        /// <param name="dataPrecision">Sets how fine grained the data will be</param>
        public Watcher(string appName, Resolution chosenResolution,
                                    SavePreference preference, DataPrecision dataPrecision)
        {
            ISaveService saveService = new SaveService(appName, preference, dataPrecision);
            IUsageKeeper keeper = CreateKeeper(ref saveService, dataPrecision, chosenResolution);
            IStorage store = new UsageStorage(ref keeper, ref saveService);

            wService = new WatcherService(ref store);
        }

        public TimeSpan UsageTimeForGivenTimeframe(DateTime startTime, DateTime endTime)
        {
            return wService.UsageTimeForGivenTimeframe(startTime, endTime);
        }

        public List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime)
        {
            return wService.BlocksOfContinousUsageForTimeFrame(startTime, endTime);
        }

        public List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, 
                                                                                                                TimeSpan maxAllowedGapInMillis)
        {
            return wService.BlocksOfContinousUsageForTimeFrame(startTime, endTime, maxAllowedGapInMillis);
        }

        public List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime)
        {
            return wService.BreaksInContinousUsageForTimeFrame(startTime, endTime);
        }

        public List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, 
                                                                                                                TimeSpan maxAllowedGapInMillis)
        {
            return wService.BreaksInContinousUsageForTimeFrame(startTime, endTime, maxAllowedGapInMillis);
        }

        private static IUsageKeeper CreateKeeper(ref ISaveService saveService,
                                                                    DataPrecision dataPrecision, Resolution chosenResolution)
        {
            IUsageKeeper keeper = saveService.GetSavedUsages();

            if (keeper == null || keeper?.GetCurrentResolution() != chosenResolution)
            {
                switch (dataPrecision)
                {
                    case DataPrecision.High:
                        keeper = new HighPrecisionUsageKeeper(chosenResolution);
                        break;

                    case DataPrecision.Low:
                        throw new NotImplementedException();
                    //break;

                    default:
                        break;
                }
            }

            return keeper;
        }

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                wService.Dispose();
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
