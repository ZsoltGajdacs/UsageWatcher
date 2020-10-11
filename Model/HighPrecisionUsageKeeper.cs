using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace UsageWatcher.Model
{
    [Serializable]
    internal class HighPrecisionUsageKeeper : IUsageKeeper
    {
        public Resolution Res { get; private set; }
        public ConcurrentDictionary<DateTime, List<UsageModel>> Usages { get; private set; }

        public HighPrecisionUsageKeeper(Resolution resolution)
        {
            Usages = new ConcurrentDictionary<DateTime, List<UsageModel>>();
            Res = resolution;
        }

        [JsonConstructor]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", 
            "IDE0051:Remove unused private members", Justification = "Only for Json it is used")]
        private HighPrecisionUsageKeeper(Resolution res, 
                        ConcurrentDictionary<DateTime, List<UsageModel>> usages) : this(res)
        {
            Usages = usages ?? throw new ArgumentNullException(nameof(usages));
        }

        public void AddUsage(DateTime startTime)
        {
            UsageModel use = new UsageModel(Res, startTime);

            GetUsagesOfDate(startTime.Date, out List<UsageModel> usages);

            if (!IsInRecordedTimeframe(startTime, ref usages))
            {
                usages.Add(use);
            }
        }

        public TimeSpan GetUsageForDateRange(DateTime start, DateTime finish)
        {
            GetUsagesOfDate(start.Date, out List<UsageModel> usages);

            List<UsageModel> filteredUsages = usages
            .Where(u => (u.StartTime >= start) && (u.EndTime <= finish))
            .ToList();

            int totalMillisecs = (int)Res * filteredUsages.Count;

            return TimeSpan.FromMilliseconds(totalMillisecs);
        }

        public Resolution GetResolution()
        {
            return Res;
        }

        public void EraseUsageNotOfDate(DateTime date)
        {
            GetUsagesOfDate(date.Date, out List<UsageModel> usages);

            Usages = new ConcurrentDictionary<DateTime, List<UsageModel>>();
            Usages.TryAdd(date.Date, usages);
        }

        private bool IsInRecordedTimeframe(DateTime startTime, ref List<UsageModel> usages)
        {
            DateTime endTime = startTime + TimeSpan.FromMilliseconds((double)Res);

            return usages.Where(u => u.StartTime <= startTime && u.EndTime >= endTime).Any();
        }

        private void GetUsagesOfDate(DateTime date, out List<UsageModel> usages)
        {
            bool hasValue = Usages.TryGetValue(date.Date, out usages);
            if (!hasValue)
            {
                usages = new List<UsageModel>();
                Usages.TryAdd(date.Date, usages);
            }
        }
    }
}
