using System;

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
    }
}
