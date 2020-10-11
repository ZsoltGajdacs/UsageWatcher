using System;
using UsageWatcher.Events;
using UsageWatcher.Model;

namespace UsageWatcher.Storage
{
    internal interface IStorage : IDisposable
    {
        event TimerElaspedEventHandler TimerElasped;

        void AddUsage();
        IUsageKeeper GetUsageKeeper();
        Resolution GetResolution();
        DateTime GetStartupTime();
    }
}
