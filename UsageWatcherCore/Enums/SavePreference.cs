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
        NoSave,
        [Description("Data is saved for a single day.")]
        [Display(Name = "Save Today")]
        KeepDataForToday,
        [Description("Data is saved for every day, can be recalled any time.")]
        [Display(Name = "Save All")]
        KeepDataForever
    }
}
