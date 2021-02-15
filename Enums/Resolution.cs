using System;
using System.ComponentModel.DataAnnotations;

namespace UsageWatcher.Enums
{
    [Serializable]
    public enum Resolution
    {
        [Display(Name = "1/2 min")]
        HalfMinute = 30000,
        [Display(Name = "1 min")]
        OneMinute = 60000,
        [Display(Name = "2 min")]
        TwoMinutes = 120000,
        [Display(Name = "3 min")]
        ThreeMinutes = 180000,
        [Display(Name = "4 min")]
        FourMinutes = 240000,
        [Display(Name = "5 min")]
        FiveMinutes = 3000000,
        [Display(Name = "10 min")]
        TenMinutes = 600000,
        [Display(Name = "15 min")]
        FifteenMinutes = 900000,
        [Display(Name = "30 min")]
        ThirtyMinutes = 1800000
    }
}
