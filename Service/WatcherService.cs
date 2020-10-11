using System;
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

        #region Event Handlers
        private void Store_TimerElasped()
        {
            saveService.Save(store.GetUsageKeeper(), store.GetStartupTime());
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
