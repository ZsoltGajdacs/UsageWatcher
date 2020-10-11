using System;
using UsageWatcher.Model;

namespace UsageWatcher.Service
{
    internal interface ISaveService
    {
        void Save(IUsageKeeper usages, DateTime startupTime);
        IUsageKeeper GetSavedUsages();
        SavePreference GetSavePreference();
        DataPrecision GetDataPrecision();
    }
}
