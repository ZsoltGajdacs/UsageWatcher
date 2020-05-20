using System;
using System.Collections.Generic;
using System.Timers;
using UsageWatcher.Model;

namespace UsageWatcher.Storage
{
    internal class UsageStore : IDisposable
    {
        private Resolution _resolution;

        private List<UsageModel> usage;
        private Timer resolutionTimer;

        internal bool IsInLockdown { get; set; }

        public Resolution ChosenResolution { get => _resolution; }

        public UsageStore(Resolution resolution)
        {
            usage = new List<UsageModel>();
            _resolution = resolution;

            resolutionTimer = new Timer((int)resolution);
            resolutionTimer.Elapsed += ResolutionTimer_Elapsed;
            resolutionTimer.AutoReset = false;
            resolutionTimer.Enabled = false;
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
            int totalMillisecs = (int)ChosenResolution * usage.Count;
            return TimeSpan.FromMilliseconds(totalMillisecs);
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
        public void Dispose()
        {
            ((IDisposable)resolutionTimer).Dispose();
        }
    }
}
