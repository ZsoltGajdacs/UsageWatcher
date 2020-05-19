using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsageWatcher.Model
{
    internal class UsageModel
    {
        internal Resolution Resolution { get; set; }
        internal DateTime StartTime { get; set; }
        internal DateTime EndTime { get; set; }
        internal bool WasMouseUsed { get; set; }
        internal bool WasKeyboardUsed { get; set; }

        public UsageModel(Resolution resolution, DateTime startTime)
        {
            Resolution = resolution;
            StartTime = startTime;
        }

        public bool WasActive()
        {
            bool wasUsed = false;

            if (WasMouseUsed)
            {
                wasUsed = true;
            }
            else if (WasKeyboardUsed)
            {
                wasUsed = true;
            }

            return wasUsed;
        }
    }
}
