using System;

namespace UsageWatcher.Model
{
    internal interface IUsageKeeper
    {
        void AddUsage(DateTime startTime);
        TimeSpan GetUsageForDateRange(DateTime start, DateTime finish);
        Resolution GetResolution();
        void EraseUsageNotOfDate(DateTime date);
    }
}
