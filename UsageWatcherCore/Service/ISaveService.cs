using UsageWatcher.Enums;
using UsageWatcher.Models;

namespace UsageWatcher.Service
{
    internal interface ISaveService
    {
        void Save(IUsageKeeper keeper);
        IUsageKeeper GetSavedUsages();
        SavePreference GetSavePreference();
        DataPrecision GetDataPrecision();
    }
}
