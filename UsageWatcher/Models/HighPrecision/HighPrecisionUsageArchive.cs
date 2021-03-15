using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private HighPrecisionUsageArchive(Dictionary<DateTime, List<HighPrecisionUsageModel>> todaysUsage) 
            : base(todaysUsage)
        {
        }
        #endregion
    }
}
