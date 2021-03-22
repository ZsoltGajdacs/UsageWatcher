using System;
using System.Timers;
using UsageWatcher.Events;

namespace UsageWatcher.Models
{
    internal class SaveTimer : IDisposable
    {
        private readonly Timer saveTimer;

        internal event TimerElaspedEventHandler Elapsed;

        public SaveTimer()
        {
            saveTimer = new Timer();
            saveTimer.AutoReset = false;
            saveTimer.Enabled = false;
            saveTimer.Elapsed += SaveTimer_Elapsed;
        }

        public void StartOnce(double interval)
        {
            saveTimer.Interval = interval;

            if (!saveTimer.Enabled)
            {
                saveTimer.Start();
            }
        }

        private void SaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Elapsed?.Invoke();
        }

        public void Dispose()
        {
            ((IDisposable)saveTimer).Dispose();
        }
    }
}
