using System;
using System.Timers;
using UsageWatcher.Events;
using UsageWatcher.Enums;

namespace UsageWatcher.Models
{
    internal class UsageTimer : IDisposable
    {
        private readonly Timer resolutionTimer;

        internal bool IsActive { get => resolutionTimer.Enabled; }

        internal event ResolutionPassedEventHandler Elasped;

        public UsageTimer(Resolution resolution)
        {
            resolutionTimer = new Timer((int)resolution);
            resolutionTimer.Elapsed += ResolutionTimer_Elapsed;
            resolutionTimer.AutoReset = false;
            resolutionTimer.Enabled = false;
        }

        public void StartOnce()
        {
            if (!resolutionTimer.Enabled)
            {
                resolutionTimer.Start();
            }
        }

        private void ResolutionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Elasped?.Invoke();
        }

        public void Dispose()
        {
            ((IDisposable)resolutionTimer).Dispose();
        }
    }
}
