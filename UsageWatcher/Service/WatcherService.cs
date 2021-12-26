using System;
using System.Collections.Generic;
using UsageWatcher.Enums;
using UsageWatcher.Models;
using UsageWatcher.Native;
using UsageWatcher.Storage;

namespace UsageWatcher.Service
{
    internal class WatcherService : IWatcher, IDisposable
    {
        private readonly KeyboardHook keyboard;
        private readonly MouseDetector mouse;

        private readonly IStorage store;

        #region CTOR
        public WatcherService(ref IStorage store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));

            keyboard = new KeyboardHook(KeyboardHook.Parameters.PassAllKeysToNextApp);
            keyboard.KeyIntercepted += Keyboard_KeyIntercepted;

            mouse = new MouseDetector(store.GetCurrentResolution(), store.GetDataPrecision());
            mouse.MouseMoved += Mouse_MouseMoved;
        }
        #endregion

        #region Interface methods
        public TimeSpan UsageTimeForGivenTimeframe(DateTime startTime, DateTime endTime)
        {
            return store.UsageTimeForGivenTimeframe(startTime, endTime);
        }

        public List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime)
        {
            double resInMs = (double)store.GetCurrentResolution();
            TimeSpan maxGap = TimeSpan.FromMilliseconds(resInMs + resInMs / 4);
            return store.BlocksOfContinousUsageForTimeFrame(startTime, endTime, maxGap);
        }

        public List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, 
                                                                                                                TimeSpan maxAllowedGapInMillis)
        {
            return store.BlocksOfContinousUsageForTimeFrame(startTime, endTime, maxAllowedGapInMillis);
        }

        public List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime)
        {
            double resInMs = (double)store.GetCurrentResolution();
            TimeSpan maxGap = TimeSpan.FromMilliseconds(resInMs + resInMs / 4);
            return store.BreaksInContinousUsageForTimeFrame(startTime, endTime, maxGap);
        }

        public List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, 
                                                                         TimeSpan maxAllowedGapInMillis)
        {
            return store.BreaksInContinousUsageForTimeFrame(startTime, endTime, maxAllowedGapInMillis);
        }

        public List<DateTime> ListOfDaysWithUsageData()
        {
            return store.ListDatesWithData();
        }

        public void SetNewResolution(Resolution resolution)
        {
            store.SetCurrentResolution(resolution);
        }
        #endregion

        #region Event Handlers
        private void Mouse_MouseMoved(object sender, System.Windows.Point p)
        {
            store.AddUsage();
        }

        private void Keyboard_KeyIntercepted(object sender, KeyboardHook.KeyboardHookEventArgs e)
        {
            store.AddUsage();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            store.Dispose();
            keyboard.Dispose();
            mouse.Dispose();
        }
        #endregion
    }
}
