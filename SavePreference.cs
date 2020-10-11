using System;
using System.ComponentModel;

namespace UsageWatcher
{
    [Serializable]
    public enum SavePreference
    {
        [Description("No data will be saved, if the app containing the lib is shut down, the data is lost.")]
        NoSave,
        [Description("Data is saved for a single day.")]
        KeepDataForToday,
        [Description("Data is saved permanently, can be recalled any time.")]
        KeepDataForever
    }
}
