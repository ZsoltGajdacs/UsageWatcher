using Newtonsoft.Json;
using System;
using UsageWatcher.Enums;

namespace UsageWatcher.Model
{
    [Serializable]
    internal class UsageModel
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public UsageModel(Resolution resolution, DateTime startTime)
        {
            StartTime = startTime;
            EndTime = startTime + TimeSpan.FromMilliseconds((double)resolution);
        }

        [JsonConstructor]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality",
            "IDE0051:Remove unused private members", Justification = "Only for Json it is used")]
        private UsageModel(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
