using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UsageWatcher.Enums;

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
            List<UsageModel> filteredUsages = ComposeListOfUsages(start, finish);

            int totalMillisecs = (int)Res * filteredUsages.Count;

            return TimeSpan.FromMilliseconds(totalMillisecs);
        }

        public List<UsageBlock> UsageListForGivenTimeFrame(DateTime startTime, DateTime endTime)
        {
            List<UsageModel> filteredUsages = ComposeListOfUsages(startTime, endTime);

            List<UsageBlock> blockList = new List<UsageBlock>();
            UsageBlock block = new UsageBlock();
            TimeSpan maxAllowedGapInMillis = TimeSpan.FromMilliseconds((int)Res + (int)Res / 4);
            foreach (UsageModel usage in filteredUsages)
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

        public List<UsageBlock> NotUsageListForGivenTimeFrame(DateTime startTime, DateTime endTime)
        {
            List<UsageBlock> usageList = UsageListForGivenTimeFrame(startTime, endTime);

            List<UsageBlock> notUsageList = new List<UsageBlock>();
            UsageBlock notUsageBlock = new UsageBlock();
            bool isValueWrittenInRound;
            foreach (UsageBlock block in usageList)
            {
                isValueWrittenInRound = false;

                if (notUsageBlock.StartTime == default && notUsageBlock.EndTime == default 
                    && !isValueWrittenInRound)
                {
                    notUsageBlock.StartTime = block.EndTime;
                    isValueWrittenInRound = true;
                }

                if (notUsageBlock.EndTime == default && notUsageBlock.StartTime != default 
                    && !isValueWrittenInRound)
                {
                    notUsageBlock.EndTime = block.StartTime;
                    isValueWrittenInRound = true;
                }

                if (notUsageBlock.StartTime != default && notUsageBlock.EndTime != default 
                    && isValueWrittenInRound)
                {
                    notUsageList.Add(notUsageBlock);
                    notUsageBlock = new UsageBlock
                    {
                        StartTime = block.EndTime
                    };
                }
            }

            return notUsageList;
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

        private List<UsageModel> ComposeListOfUsages(DateTime start, DateTime finish)
        {
            GetUsagesOfDate(start.Date, out List<UsageModel> usages);

            return usages
            .Where(u => (u.StartTime >= start) && (u.EndTime <= finish))
            .ToList();
        }

        private bool IsInRecordedTimeframe(DateTime startTime, ref List<UsageModel> usages)
        {
            DateTime endTime = startTime + TimeSpan.FromMilliseconds((double)Res);

            return usages.Any(u => u.StartTime <= startTime && u.EndTime >= endTime);
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
