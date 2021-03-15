using System;
using System.Collections.Generic;
using UsageWatcher.Models;

namespace UsageWatcher
{
    public interface IWatcher : IDisposable
    {
        /// <summary>
        /// Gives back usage time inbetween the given dates - times
        /// </summary>
        TimeSpan UsageTimeForGivenTimeframe(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gives back a list which contains the start-end times of continous usages. 
        /// The maximum amount of time between two usages, that is still considered as one block
        /// is the currently set (resolution + resolution/4)
        /// </summary>
        List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gives back a list which contains the start-end times of continous usages. 
        /// Here you can set the max length between separate usages that will still be considered a block.
        /// Ex: If set to 10 minutes then if you used the computer from 9:00AM to 9:05AM and then again from 9:15AM to 9:20AM
        /// it will be considered as an block
        /// </summary>
        List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, 
                                                                                                    TimeSpan maxAllowedGapInMillis);

        /// <summary>
        /// The inverse of the continous usageblock list: gives back the times between continous usages
        /// </summary>
        List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime);

        /// <summary>
        /// The inverse of the continous usageblock list: gives back the times between continous usages
        /// Here you can set the max length between separate usages that will still be considered a block. 
        /// (And you're getting the breaks in that)
        /// </summary>
        List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, 
                                                                                                    TimeSpan maxAllowedGapInMillis);
    }
}
