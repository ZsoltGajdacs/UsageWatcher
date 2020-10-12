using System;

namespace UsageWatcher
{
    [Serializable]
    public enum Resolution
    {
        HalfMinute = 30000,
        OneMinute = 60000,
        TwoMinutes = 120000,
        ThreeMinutes = 180000,
        FourMinutes = 240000,
        FiveMinutes = 3000000,
        TenMinutes = 600000,
        FifteenMinutes = 900000,
        ThirtyMinutes = 1800000
    }
}
