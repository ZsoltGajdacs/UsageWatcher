using System;
using System.Collections.Generic;

namespace UsageWatcher.Models
{
    internal interface IUsageKeeper
    {
        TimeSpan UsageTimeForGivenTimeframe(DateTime start, DateTime finish);
        List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, TimeSpan maxAllowedGapInMillis);
        List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, TimeSpan maxAllowedGapInMillis);
    }
}
