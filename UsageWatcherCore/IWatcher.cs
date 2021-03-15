using System;
using System.Collections.Generic;
using UsageWatcher.Models;

namespace UsageWatcher
{
    public interface IWatcher : IDisposable
    {
        /// <summary>
        /// Gives back usage time inbetween the given dates
        /// </summary>
        TimeSpan UsageForGivenTimeframe(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gives back a list which contains the start-end times of continous usages
        /// </summary>
        List<UsageBlock> UsageListForGivenTimeFrame(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gives back the list of start - end time betwen which the computer was NOT used
        /// </summary>
        List<UsageBlock> NotUsageListForGivenTimeFrame(DateTime startTime, DateTime endTime);
    }
}
