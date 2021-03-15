using Microsoft.Win32;
using System;
using UsageWatcher.Events;
using UsageWatcher.Models;
using UsageWatcher.Enums;

namespace UsageWatcher.Storage
{
    internal class UsageStore : IStorage
    {
        private readonly IUsageKeeper usageKeeper;

        private readonly UsageTimer timer;
        private readonly DateTime startupTime;
        private bool isInLockdown;

        public event TimerElaspedEventHandler TimerElasped;

        public UsageStore(IUsageKeeper usageKeeper)
        {
            this.usageKeeper = usageKeeper;

            timer = new UsageTimer(usageKeeper.GetResolution());
            startupTime = DateTime.Now;

            timer.Elasped += Timer_Elasped;
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(OnWindowsLockUnlock);
        }

        public void AddUsage()
        {
            if (!isInLockdown && HasLastTimeFramePassed())
            {
                usageKeeper.AddUsage(DateTime.Now);

                timer.Start();
            }
        }

        public IUsageKeeper GetUsageKeeper()
        {
            return usageKeeper;
        }

        public Resolution GetResolution()
        {
            return usageKeeper.GetResolution();
        }

        public DateTime GetStartupTime()
        {
            return startupTime;
        }

        private bool HasLastTimeFramePassed()
        {
            return !timer.IsActive;
        }

        private void Timer_Elasped()
        {
            TimerElasped?.Invoke();
        }

        private void OnWindowsLockUnlock(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                isInLockdown = true;
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                isInLockdown = false;
            }
        }

        public void Dispose()
        {
            ((IDisposable)timer).Dispose();
        }
    }
}
