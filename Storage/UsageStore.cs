using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UsageWatcher.Model;

namespace UsageWatcher.Storage
{
    internal class UsageStore : IDisposable
    {
        private Resolution _resolution;

        private bool saveToTempStore;
        private List<UsageModel> usage;
        private Timer resolutionTimer;
        private DateTime startupTime;
        private DateTime lastSyncDate;

        internal bool IsInLockdown { get; set; }

        public Resolution ChosenResolution { get => _resolution; }

        public UsageStore(Resolution resolution, bool saveDataToTempStorage)
        {
            usage = new List<UsageModel>();
            _resolution = resolution;
            saveToTempStore = saveDataToTempStorage;
            startupTime = DateTime.Now;

            resolutionTimer = new Timer((int)resolution);
            resolutionTimer.Elapsed += ResolutionTimer_Elapsed;
            resolutionTimer.AutoReset = false;
            resolutionTimer.Enabled = false;

            SetupTempSave(saveToTempStore);
        }

        public void AddUsage(UsageType type)
        {
            DateTime now = DateTime.Now;

            if (!IsInLockdown)
            {
                if (HasLastTimeFramePassed(now))
                {
                    UsageModel model = new UsageModel(ChosenResolution, now);
                    usage.Add(model);

                    resolutionTimer.Start();
                }
                else
                {
                    var model = GetLastUsage();

                    switch (type)
                    {
                        case UsageType.KEYBOARD:
                            if (!model.WasKeyboardUsed)
                            {
                                model.WasKeyboardUsed = true;
                            }
                            break;

                        case UsageType.MOUSE:
                            if (!model.WasMouseUsed)
                            {
                                model.WasMouseUsed = true;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public TimeSpan CalculateUsageSoFar()
        {
            lastSyncDate = DateTime.Now;
            int totalMillisecs = (int)ChosenResolution * usage.Count;
            return TimeSpan.FromMilliseconds(totalMillisecs);
        }

        //TODO: Create a more fine grained check based on the usagemodel resolutions. 
        //This would make it possible to slice usages if they intersect with start / end times
        public TimeSpan CalculateUsageSoFar(DateTime startTime, DateTime endTime)
        {
            lastSyncDate = DateTime.Now;
            List<UsageModel> filteredUsages = usage
                .Where(u => (u.StartTime >= startTime) && (u.EndTime <= endTime))
                .ToList();

            int totalMillisecs = (int)ChosenResolution * filteredUsages.Count;
            return TimeSpan.FromMilliseconds(totalMillisecs);
        }

        /// <summary>
        /// Returns usage data since the last sync time, 
        /// or if there never was a sync than since startup
        /// </summary>
        /// <returns></returns>
        public TimeSpan CalculateUsageSinceLastSync()
        {
            DateTime startTime = lastSyncDate == default ? startupTime : lastSyncDate;
            return CalculateUsageSoFar(startTime, DateTime.Now);
        }

        private UsageModel GetLastUsage()
        {
            return usage[usage.Count - 1];
        }

        private bool HasLastTimeFramePassed(DateTime time)
        {
            return !resolutionTimer.Enabled;
        }

        private void ResolutionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UsageModel lastUsage = GetLastUsage();
            lastUsage.EndTime = DateTime.Now;
        }

        private static void SetupTempSave(bool saveToTempStore)
        {
            if (saveToTempStore)
            {

            }
        }

        internal static string GetTempStorageFileLocation()
        {
            return GetTempDirLocation() + "usagestore.json";
        }

        internal static string GetTempDirLocation()
        {
            string userAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return userAppData + "\\Usagewatcher";
        }

        public void Dispose()
        {
            ((IDisposable)resolutionTimer).Dispose();
        }
    }
}
