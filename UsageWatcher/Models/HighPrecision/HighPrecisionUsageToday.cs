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
        public Resolution CurrentResolution { get; private set; }

        #region CTORs
        public HighPrecisionUsageToday(Resolution res) : base()
        {
            CurrentResolution = res;
        }

        [JsonConstructor]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality",
            "IDE0051:Remove unused private members", Justification = "Only for Json it is used")]
        private HighPrecisionUsageToday(Resolution res,
            Dictionary<DateTime, List<HighPrecisionUsageModel>> todaysUsage) : base(todaysUsage)
        {
            CurrentResolution = res;
        }
        #endregion

        public void AddUsage(DateTime startTime)
        {
            HighPrecisionUsageModel use = new HighPrecisionUsageModel(CurrentResolution, startTime);

            GetUsagesOfDate(startTime.Date, out List<HighPrecisionUsageModel> usages);

            if (!IsInRecordedTimeframe(startTime, ref usages))
            {
                usages.Add(use);
            }
        }

        public IDictionary<DateTime, List<HighPrecisionUsageModel>> GetArchivableUsages()
        {
             var usagesToArchive = Usage
                                                    .Where(u => u.Key.Date < DateTime.Now.Date)
                                                    .ToDictionary(k => k.Key, v => v.Value);

            foreach (var dateElem in usagesToArchive.Keys)
            {
                Usage.Remove(dateElem);
            }

            return usagesToArchive;
        }

        public void SetCurrentResolution(Resolution newRes)
        {
            CurrentResolution = newRes;
        }

        public Resolution GetCurrentResolution()
        {
            return CurrentResolution;
        }

        private bool IsInRecordedTimeframe(DateTime startTime, ref List<HighPrecisionUsageModel> usages)
        {
            DateTime endTime = startTime + TimeSpan.FromMilliseconds((double)CurrentResolution);

            return usages.Any(u => u.StartTime <= startTime && u.EndTime >= endTime);
        }
    }
}
