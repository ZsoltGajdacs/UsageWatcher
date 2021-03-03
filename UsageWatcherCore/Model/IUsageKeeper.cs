using System;
using System.Collections.Generic;
using UsageWatcher.Enums;

namespace UsageWatcher.Model
{
    internal interface IUsageKeeper
    {
        void AddUsage(DateTime startTime);
        TimeSpan GetUsageForDateRange(DateTime start, DateTime finish);
        List<UsageBlock> UsageListForGivenTimeFrame(DateTime startTime, DateTime endTime);
        List<UsageBlock> NotUsageListForGivenTimeFrame(DateTime startTime, DateTime endTime);
        Resolution GetResolution();
        void EraseUsageNotOfDate(DateTime date);
    }
}
