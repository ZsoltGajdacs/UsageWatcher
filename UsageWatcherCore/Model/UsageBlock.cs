using System;

namespace UsageWatcher.Models
{
    /// <summary>
    /// An aggragate of usage times
    /// </summary>
    [Serializable]
    public class UsageBlock
    {
        public Guid Id { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public UsageBlock()
        {
            Id = Guid.NewGuid();
        }

        public UsageBlock(DateTime startTime, DateTime endTime)
        {
            Id = Guid.NewGuid();
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
