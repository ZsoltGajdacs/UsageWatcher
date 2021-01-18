using System;
using System.Collections.Generic;
using UsageWatcher.Model;
using UsageWatcher.Native;
using UsageWatcher.Storage;

namespace UsageWatcher.Service
{
    internal class WatcherService : IWatcher, IDisposable
    {
        private readonly KeyboardHook keyboard;
        private readonly MouseDetector mouse;

        private readonly IStorage store;
        private readonly ISaveService saveService;

        public WatcherService(ref IStorage store, ref ISaveService saveService)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            this.saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));

            store.TimerElasped += Store_TimerElasped;

            keyboard = new KeyboardHook(KeyboardHook.Parameters.PassAllKeysToNextApp);
            keyboard.KeyIntercepted += Keyboard_KeyIntercepted;

            mouse = new MouseDetector(store.GetResolution(), saveService.GetDataPrecision());
            mouse.MouseMoved += Mouse_MouseMoved;
        }

        public TimeSpan UsageForGivenTimeframe(DateTime startTime, DateTime endTime)
        {
            return store.GetUsageKeeper().GetUsageForDateRange(startTime, endTime);
        }

        public List<UsageBlock> UsageListForGivenTimeFrame(DateTime startTime, DateTime endTime)
        {
            return store.GetUsageKeeper().UsageListForGivenTimeFrame(startTime, endTime);
        }

        public List<UsageBlock> NotUsageListForGivenTimeFrame(DateTime startTime, DateTime endTime)
        {
            return store.GetUsageKeeper().NotUsageListForGivenTimeFrame(startTime, endTime);
        }

        #region Event Handlers
        private void Store_TimerElasped()
        {
            saveService.Save(store.GetUsageKeeper());
        }

        private void Mouse_MouseMoved(object sender, System.Windows.Point p)
        {
            store.AddUsage();
        }

        private void Keyboard_KeyIntercepted(object sender, KeyboardHook.KeyboardHookEventArgs e)
        {
            store.AddUsage();
        }
        #endregion

        public void Dispose()
        {
            store.Dispose();
            keyboard.Dispose();
            mouse.Dispose();
        }
    }
}
