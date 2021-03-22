using System;
using UsageWatcher.Models;
using UsageWatcher.Enums;
using System.Collections.Generic;

namespace UsageWatcher.Storage
{
    internal interface IStorage : IDisposable
    {
        void AddUsage();
        TimeSpan UsageTimeForGivenTimeframe(DateTime start, DateTime finish);
        List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, TimeSpan maxAllowedGapInMillis);
        List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, TimeSpan maxAllowedGapInMillis);
        void SetCurrentResolution(Resolution newRes);
        Resolution GetCurrentResolution();
        DataPrecision GetDataPrecision();
    }
}
