using System;

namespace UsageWatcher
{
    public interface IWatcher : IDisposable
    {
        /// <summary>
        /// Gives back usage time inbetween the given dates
        /// </summary>
        TimeSpan UsageForGivenTimeframe(DateTime startTime, DateTime endTime);
    }
}
