using System.ComponentModel;

namespace UsageWatcher
{
    public enum DataPrecision
    {
        [Description("Data is kept for every usages, having a detailed record as per chosen resolution")]
        HighPrecision,
        [Description("Not Implemented yet")]
        LowPrecision
    }
}
