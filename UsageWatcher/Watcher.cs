using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UsageWatcher.Enums;
using UsageWatcher.Models;
using UsageWatcher.Models.HighPrecision;
using UsageWatcher.Service;
using UsageWatcher.Storage;

[assembly: InternalsVisibleTo("UsageWatcherTest")]
namespace UsageWatcher
{
    /// <summary>
    /// Watches the system for mouse and keyboard movements and measures the time
    /// </summary>
    public class Watcher : IWatcher, IDisposable
    {
        private readonly WatcherService wService;

        #region CTOR
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
            IUsageToday today = (IUsageToday)CreateOrLoadKeeper(ref saveService, dataPrecision, 
                                                                                                        chosenResolution, SaveType.Today);
            IUsageArchive archive = (IUsageArchive)CreateOrLoadKeeper(ref saveService, dataPrecision, 
                                                                                                                chosenResolution, SaveType.Archive);
            IStorage store = new UsageStorage(ref today, ref archive, ref saveService);

            wService = new WatcherService(ref store);
        }
        #endregion

        #region Interface methods
        public TimeSpan UsageTimeForGivenTimeframe(DateTime startTime, DateTime endTime)
        {
            return wService.UsageTimeForGivenTimeframe(startTime, endTime);
        }

        public List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime)
        {
            return wService.BlocksOfContinousUsageForTimeFrame(startTime, endTime);
        }

        public List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, TimeSpan maxAllowedGapInMillis)
        {
            return wService.BlocksOfContinousUsageForTimeFrame(startTime, endTime, maxAllowedGapInMillis);
        }

        public List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime)
        {
            return wService.BreaksInContinousUsageForTimeFrame(startTime, endTime);
        }

        public List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, TimeSpan maxAllowedGapInMillis)
        {
            return wService.BreaksInContinousUsageForTimeFrame(startTime, endTime, maxAllowedGapInMillis);
        }

        public List<DateTime> ListOfDaysWithUsageData()
        {
            return wService.ListOfDaysWithUsageData();
        }

        public void SetNewResolution(Resolution resolution)
        {
            wService.SetNewResolution(resolution);
        }
        #endregion

        #region Helpers
        private static IUsageKeeper CreateOrLoadKeeper(ref ISaveService saveService, 
            DataPrecision dataPrecision, Resolution chosenResolution, SaveType saveType)
        {
            IUsageKeeper keeper = saveService.GetSavedUsages(saveType);

            if (keeper == null)
            {
                if (dataPrecision == DataPrecision.High)
                {
                    if (saveType == SaveType.Today)
                    {
                        keeper = new HighPrecisionUsageToday(chosenResolution);
                    } else
                    {
                        keeper = new HighPrecisionUsageArchive();
                    }
                } else
                {
                    throw new NotImplementedException();
                }
            }

            return keeper;
        }
        #endregion

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
