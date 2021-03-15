using System;
using System.Collections.Generic;
using System.Linq;

namespace UsageWatcher.Models
{
    [Serializable]
    internal abstract class HighPrecisionUsageKeeper : IUsageKeeper
    {
        public IDictionary<DateTime, List<HighPrecisionUsageModel>> Usage { get; private set; }

        #region CTORS
        protected HighPrecisionUsageKeeper()
        {
            Usage = new Dictionary<DateTime, List<HighPrecisionUsageModel>>();
        }

        protected HighPrecisionUsageKeeper(IDictionary<DateTime, List<HighPrecisionUsageModel>> usage)
        {
            Usage = usage ?? throw new ArgumentNullException(nameof(usage));
        }
        #endregion

        #region Interface methods
        public TimeSpan UsageTimeForGivenTimeframe(DateTime start, DateTime finish)
        {
            List<HighPrecisionUsageModel> filteredUsages = ComposeListOfUsages(start, finish);

            int totalMillisecs = 0;
            foreach (HighPrecisionUsageModel usageModel in filteredUsages)
            {
                totalMillisecs += (int)usageModel.Resolution;
            }

            return TimeSpan.FromMilliseconds(totalMillisecs);
        }

        public List<UsageBlock> BlocksOfContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, 
                                                                                                TimeSpan maxAllowedGapInMillis)
        {
            List<HighPrecisionUsageModel> filteredUsages = ComposeListOfUsages(startTime, endTime);

            List<UsageBlock> blockList = new List<UsageBlock>();
            UsageBlock block = new UsageBlock();
            foreach (HighPrecisionUsageModel usage in filteredUsages)
            {
                if (block.StartTime == default && block.EndTime == default)
                {
                    block.StartTime = usage.StartTime;
                    block.EndTime = usage.EndTime;
                }

                if ((usage.StartTime - block.EndTime) <= maxAllowedGapInMillis)
                {
                    block.EndTime = usage.EndTime;
                }
                else
                {
                    blockList.Add(block);
                    block = new UsageBlock(usage.StartTime, usage.EndTime);
                }
            }

            if (!blockList.Any(sm => sm.Id == block.Id))
            {
                blockList.Add(block);
            }

            return blockList;
        }

        public List<UsageBlock> BreaksInContinousUsageForTimeFrame(DateTime startTime, DateTime endTime, 
                                                                                                    TimeSpan maxAllowedGapInMillis)
        {
            List<UsageBlock> usageList = BlocksOfContinousUsageForTimeFrame(startTime, endTime, maxAllowedGapInMillis);

            List<UsageBlock> notUsageList = new List<UsageBlock>();
            UsageBlock notUsageBlock = new UsageBlock();
            foreach (UsageBlock usageBlock in usageList)
            {
                if (notUsageBlock.StartTime == default && notUsageBlock.EndTime == default)
                {
                    notUsageBlock.StartTime = usageBlock.EndTime;
                } 
                else if (notUsageBlock.StartTime != default && notUsageBlock.EndTime == default)
                {
                    notUsageBlock.EndTime = usageBlock.StartTime;
                }

                if (notUsageBlock.StartTime != default && notUsageBlock.EndTime != default)
                {
                    notUsageList.Add(notUsageBlock);
                    notUsageBlock = new UsageBlock
                    {
                        StartTime = usageBlock.EndTime
                    };
                }
            }

            return notUsageList;
        }
        #endregion

        protected List<HighPrecisionUsageModel> ComposeListOfUsages(DateTime start, DateTime finish)
        {
            GetUsagesOfDate(start.Date, out List<HighPrecisionUsageModel> usages);

            return usages
            .Where(u => (u.StartTime >= start) && (u.EndTime <= finish))
            .ToList();
        }

        protected void GetUsagesOfDate(DateTime date, out List<HighPrecisionUsageModel> usages)
        {
            bool hasValue = Usage.TryGetValue(date.Date, out usages);
            if (!hasValue)
            {
                usages = new List<HighPrecisionUsageModel>();
                Usage.Add(date.Date, usages);
            }
        }
    }
}
