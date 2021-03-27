using Newtonsoft.Json;
using System;
using UsageWatcher.Enums;

namespace UsageWatcher.Models.HighPrecision
{
    [Serializable]
    public class HighPrecisionUsageModel
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public Resolution Resolution { get; private set; }

        public HighPrecisionUsageModel(Resolution resolution, DateTime startTime)
        {
            StartTime = startTime;
            EndTime = CalcEndtime(startTime, resolution);
            Resolution = resolution;
        }

        [JsonConstructor]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality",
            "IDE0051:Remove unused private members", Justification = "Only for Json it is used")]
        private HighPrecisionUsageModel(DateTime startTime, DateTime endTime, Resolution resolution)
        {
            if (IsValid(startTime, endTime, resolution))
            {
                StartTime = startTime;
                EndTime = endTime;
                Resolution = resolution;
            }
            else
            {
                throw new ArgumentException("Start and endtimes are not matched to the given resolution");
            }
        }

        private bool IsValid(DateTime startTime, DateTime endTime, Resolution resolution)
        {
            return CalcEndtime(startTime, resolution) == endTime;
        }

        private DateTime CalcEndtime(DateTime startTime, Resolution resolution)
        {
            return startTime + TimeSpan.FromMilliseconds((double)resolution);
        }
    }
}
