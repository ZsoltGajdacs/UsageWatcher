using System;
using System.Timers;
using UsageWatcher.Events;

namespace UsageWatcher.Models
{
    internal class SaveTimer : IDisposable
    {
        private readonly Timer saveTimer;

        internal event TimerElaspedEventHandler Elasped;

        public SaveTimer()
        {
            saveTimer.AutoReset = false;
            saveTimer.Enabled = false;
            saveTimer.Elapsed += SaveTimer_Elapsed;
        }

        public void StartOnce(double interval)
        {
            saveTimer.Interval = interval;
            saveTimer.Start();
        }

        private void SaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Elasped?.Invoke();
        }

        public void Dispose()
        {
            ((IDisposable)saveTimer).Dispose();
        }
    }
}
