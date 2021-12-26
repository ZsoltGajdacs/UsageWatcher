using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UsageWatcher.Enums
{
    [Serializable]
    public enum SavePreference
    {
        [Description("No data will be saved, if the app containing the lib is shut down, the data is lost.")]
        [Display(Name = "No data save")]
        NoSave = -1,
        [Description("Data is saved for a single day.")]
        [Display(Name = "Save Today")]
        KeepDataForToday = 0,
        [Description("Data is saved for seven days.")]
        [Display(Name = "Save 7 days")]
        KeepDataForAWeek = 7,
        [Description("Data is saved for eleven days.")]
        [Display(Name = "Save 14 days")]
        KeepDataForTwoWeeks = 14,
        [Description("Data is saved for twenty-eight days.")]
        [Display(Name = "Save 28 days")]
        KeepDataForFourWeeks = 28,
        [Description("Data is saved for fifty-six days.")]
        [Display(Name = "Save 56 days")]
        KeepDataForEightWeeks = 56,
        [Description("Data is saved for half a year.")]
        [Display(Name = "Save 183 days")]
        KeepDataForHalfAYear = 183,
        [Description("Data is kept for a whole year.")]
        [Display(Name = "Save 1 year")]
        KeepDataForAYear = 365
    }
}
