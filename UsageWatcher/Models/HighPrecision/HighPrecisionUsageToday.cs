using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UsageWatcher.Enums;

namespace UsageWatcher.Models.HighPrecision
{
    [Serializable]
    internal class HighPrecisionUsageToday : HighPrecisionUsageKeeper, IUsageToday
    {
        public Resolution Res { get; private set; }

        public HighPrecisionUsageToday(Resolution res) : base()
        {
            Res = res;
        }

        [JsonConstructor]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality",
            "IDE0051:Remove unused private members", Justification = "Only for Json it is used")]
        private HighPrecisionUsageToday(Resolution res,
            Dictionary<DateTime, List<HighPrecisionUsageModel>> todaysUsage) : base(todaysUsage)
        {
            Res = res;
        }

        public void AddUsage(DateTime startTime)
        {
            HighPrecisionUsageModel use = new HighPrecisionUsageModel(Res, startTime);

            GetUsagesOfDate(startTime.Date, out List<HighPrecisionUsageModel> usages);

            if (!IsInRecordedTimeframe(startTime, ref usages))
            {
                usages.Add(use);
            }
        }

        public Resolution GetCurrentResolution()
        {
            return Res;
        }

        private bool IsInRecordedTimeframe(DateTime startTime, ref List<HighPrecisionUsageModel> usages)
        {
            DateTime endTime = startTime + TimeSpan.FromMilliseconds((double)Res);

            return usages.Any(u => u.StartTime <= startTime && u.EndTime >= endTime);
        }
    }
}
