using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UsageWatcher.Model;

namespace UsageWatcher
{
    internal class UsageStore
    {
        private Resolution _resolution;

        private List<UsageModel> usage;
        private Timer resolutionTimer;

        public Resolution Resolution { get => _resolution; }

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

            if (HasLastTimeFramePassed(now))
            {
                UsageModel model = new UsageModel(Resolution, now);
                usage.Add(model);

                resolutionTimer.Start();
            }
            else
            {
                var model = GetLastUsage();

                switch (type)
                {
                    case UsageType.KEYBOARD:
                        model.WasKeyboardUsed = true;
                        break;

                    case UsageType.MOUSE:
                        model.WasMouseUsed = true;
                        break;
                    
                    default:
                        break;
                }
            }
            
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
    }
}
