using Microsoft.Win32;
using System;
using UsageWatcher.Model;
using UsageWatcher.Native;
using UsageWatcher.Storage;

namespace UsageWatcher.Service
{
    internal class UsageService : IDisposable
    {
        private KeyboardHook keyboard;
        private MouseDetector mouse;

        private UsageStore store;

        public UsageService(UsageStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));

            keyboard = new KeyboardHook(KeyboardHook.Parameters.PassAllKeysToNextApp);
            keyboard.KeyIntercepted += Keyboard_KeyIntercepted;

            mouse = new MouseDetector(store.ChosenResolution);
            mouse.MouseMoved += Mouse_MouseMoved;

            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(OnWindowsLockUnlock);
        }

        public TimeSpan UsageSoFar()
        {
            return store.CalculateUsageSoFar();
        }

        public TimeSpan UsageForTime(DateTime startTime, DateTime endTime)
        {
            return store.CalculateUsageSoFar(startTime, endTime);
        }

        private void Mouse_MouseMoved(object sender, System.Windows.Point p)
        {
            store.AddUsage(UsageType.MOUSE);
        }

        private void Keyboard_KeyIntercepted(object sender, KeyboardHook.KeyboardHookEventArgs e)
        {
            store.AddUsage(UsageType.KEYBOARD);
        }

        private void OnWindowsLockUnlock(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                store.IsInLockdown = true;
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                store.IsInLockdown = false;
            }
        }

        public void Dispose()
        {
            store.Dispose();
            keyboard.Dispose();
            mouse.Dispose();
        }
    }
}
