using System;
using UsageWatcher.Model;

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
