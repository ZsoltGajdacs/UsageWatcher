using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UsageWatcher.Enums
{
    [Serializable]
    public enum DataPrecision
    {
        [Description("Data is kept for every usage, having a detailed record as per chosen resolution")]
        [Display(Name = "High Precision")]
        HighPrecision,
        [Description("Not Implemented yet")]
        [Display(Name = "Low Precision")]
        LowPrecision
    }
}
