using UsageWatcher.Enums;
using UsageWatcher.Models;

namespace UsageWatcher.Service
{
    internal interface ISaveService
    {
        void Save(IUsageKeeper keeper, SaveType type);
        IUsageKeeper GetSavedUsages(SaveType type);
        SavePreference GetSavePreference();
        DataPrecision GetDataPrecision();
    }
}
