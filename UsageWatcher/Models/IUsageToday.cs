using System;
using System.Collections.Generic;
using UsageWatcher.Enums;
using UsageWatcher.Models.HighPrecision;

namespace UsageWatcher.Models
{
    internal interface IUsageToday : IUsageKeeper
    {
        void AddUsage(DateTime startTime);
        IDictionary<DateTime, List<HighPrecisionUsageModel>> GetArchivableUsages();
        Resolution GetCurrentResolution();
    }
}
