using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsageWatcher.Models.HighPrecision;
using UsageWatcher.Enums;
using System;

namespace UsageWatcherTest
{
    [TestClass]
    public class HighPrecisionUsageTodayTest
    {
        private HighPrecisionUsageToday highPrecUsageToday;

        [TestInitialize]
        public void Setup()
        {
            highPrecUsageToday = new HighPrecisionUsageToday(Resolution.HalfMinute);
        }

        [TestMethod]
        public void AddUsageTest()
        {
            DateTime now = DateTime.Now;
            FillUsage(now, 10);
            TimeSpan res = highPrecUsageToday.UsageTimeForGivenTimeframe(now, now + TimeSpan.FromHours(5));

            Assert.AreEqual(TimeSpan.FromMinutes(5), res);
        }

        private void FillUsage(DateTime startTime, int numberOfUsage)
        {
            for (int i = 0; i < numberOfUsage; i++)
            {
                highPrecUsageToday.AddUsage(startTime);
                startTime += TimeSpan.FromMilliseconds((double)Resolution.HalfMinute);
            }
        }
    }
}
