using Microsoft.Win32;
using System;
using UsageWatcher.Models;
using UsageWatcher.Enums;
using System.Collections.Generic;
using UsageWatcher.Service;
using System.Timers;

namespace UsageWatcher.Storage
{
    internal class UsageStorage : IStorage
    {
        private readonly List<SavePreference> noArchiveList;

        private readonly IUsageToday usageToday;
        private readonly IUsageArchive usageArchive;
        private readonly ISaveService saveService;

        private readonly UsageTimer usageTimer;
        private readonly SaveTimer saveTimer;

        private bool isWindowsLocked;
        

        #region CTOR
        public UsageStorage(ref IUsageToday usageToday, ref IUsageArchive usageArchive, ref ISaveService saveService)
        {
            this.usageToday = usageToday ?? throw new ArgumentNullException(nameof(usageToday));
            this.usageArchive = usageArchive ?? throw new ArgumentNullException(nameof(usageArchive));
            this.saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));

            noArchiveList = new List<SavePreference>() { SavePreference.KeepDataForToday, SavePreference.NoSave };

            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(OnWindowsLockUnlock);

            usageTimer = new UsageTimer(usageToday.GetCurrentResolution());

            saveTimer = new SaveTimer();
            saveTimer.Elapsed += SaveTimer_Elapsed;
        }
        #endregion

        #region Interface methods
        public void AddUsage()
        {
            if (!isWindowsLocked && HasLastTimeFramePassed())
            {
                usageToday.AddUsage(DateTime.Now);

                StartUsageTimer();
                StartSaveTimer();
            }
        }

        public TimeSpan UsageTimeForGivenTimeframe(DateTime start, DateTime finish)
        {
            return ChooseKeeperForDate(start.Date).UsageTimeForGivenTimeframe(start, finish);
        }

        public List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, TimeSpan maxAllowedGapInMillis)
        {
            return ChooseKeeperForDate(startTime.Date).BlocksOfContinousUsageForTimeFrame(startTime, endTime, maxAllowedGapInMillis);
        }

        public List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, TimeSpan maxAllowedGapInMillis)
        {
            return ChooseKeeperForDate(startTime.Date).BreaksInContinousUsageForTimeFrame(startTime, endTime, maxAllowedGapInMillis);
        }

        public void SetCurrentResolution(Resolution newRes)
        {
            usageToday.SetCurrentResolution(newRes);
        }

        public Resolution GetCurrentResolution()
        {
            return usageToday.GetCurrentResolution();
        }

        public DataPrecision GetDataPrecision()
        {
            return saveService.GetDataPrecision();
        }
        #endregion

        #region Support
        private IUsageKeeper ChooseKeeperForDate(DateTime date)
        {
            if (date.Date == DateTime.Today.Date)
            {
                return usageToday;
            }
            else
            {
                return usageArchive;
            }
        }

        private void ArchiveIfNeeded()
        {
            var archivableUsages = usageToday.GetArchivableUsages();
            usageArchive.Archive(archivableUsages);

            SavePreference savePreference = saveService.GetSavePreference();
            if (!noArchiveList.Contains(savePreference) && archivableUsages.Count > 0)
            {
                usageArchive.DeleteUsagesOlderThen((int)savePreference);
                saveService.Save(usageArchive, SaveType.Archive);
            }
        }

        private bool HasLastTimeFramePassed()
        {
            return !usageTimer.IsActive;
        }

        private void StartSaveTimer()
        {
            saveTimer.StartOnce(((double)GetCurrentResolution()) / 2);
        }

        private void StartUsageTimer()
        {
            usageTimer.StartOnce();
        }
        #endregion

        #region Event handlers
        private void SaveTimer_Elapsed()
        {
            ArchiveIfNeeded();
            saveService.Save(usageToday, SaveType.Today);
        }

        private void OnWindowsLockUnlock(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                isWindowsLocked = true;
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                isWindowsLocked = false;
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            ((IDisposable)usageTimer).Dispose();
            ((IDisposable)saveTimer).Dispose();
        }
        #endregion
    }
}
