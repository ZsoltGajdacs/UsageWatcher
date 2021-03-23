using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UsageWatcher.Models.HighPrecision
{
    [Serializable]
    internal class HighPrecisionUsageArchive : HighPrecisionUsageKeeper, IUsageArchive
    {
        #region CTORS
        public HighPrecisionUsageArchive()
        {
        }

        [JsonConstructor]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality",
            "IDE0051:Remove unused private members", Justification = "Only for Json it is used")]
        private HighPrecisionUsageArchive(Dictionary<DateTime, List<HighPrecisionUsageModel>> usage) 
            : base(usage)
        {
        }
        #endregion

        public void Archive(IDictionary<DateTime, List<HighPrecisionUsageModel>> usageToArchive)
        {
            foreach (var elem in usageToArchive)
            {
                Usage.Add(elem.Key, elem.Value);
            }
        }

        public void DeleteUsagesOlderThen(int numberOfDays)
        {
            if (numberOfDays < 0)
            {
                return;
            }

            DateTime dateThreshold = DateTime.Now.Date - TimeSpan.FromDays(numberOfDays);
            IEnumerable<DateTime> tooOldUsages = Usage
                                                    .Where(k => k.Key.Date < dateThreshold.Date)
                                                    .Select(k => k.Key)
                                                    .ToList();

            foreach (var usageDate in tooOldUsages)
            {
                Usage.Remove(usageDate);
            }
        }
    }
}
