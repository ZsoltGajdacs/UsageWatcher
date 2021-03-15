using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsageWatcher.Enums;

namespace UsageWatcher.Models
{
    internal interface IUsageToday : IUsageKeeper
    {
        void AddUsage(DateTime startTime);
        Resolution GetCurrentResolution();
    }
}
