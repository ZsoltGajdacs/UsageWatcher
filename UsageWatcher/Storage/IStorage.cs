using System;
using UsageWatcher.Events;
using UsageWatcher.Model;
using UsageWatcher.Enums;

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
