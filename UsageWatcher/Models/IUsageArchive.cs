using System;
using System.Collections.Generic;
using UsageWatcher.Models.HighPrecision;

namespace UsageWatcher.Models
{
    internal interface IUsageArchive : IUsageKeeper
    {
        void Archive(IDictionary<DateTime, List<HighPrecisionUsageModel>> usageToArchive);
        void DeleteUsagesOlderThen(int numberOfDays);
    }
}
