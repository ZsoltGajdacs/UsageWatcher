using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsageWatcher.Model;
using UsageWatcher.Service;

namespace UsageWatcher
{
    /// <summary>
    /// Watches the system for mouse and keyboard movements
    /// and provides you with simple booleans about system usage
    /// </summary>
    public class UsageWatcher
    {
        private UsageService uService;

        /// <summary>
        /// You must pass the smallest timeframe the software will watch for.
        /// Eg.: If set to TEN_MINUTES, then one mouse movement or keypress in that timeframe
        /// will be considered an active time
        /// </summary>
        /// <param name="resolution">The smallest timeframe the software will watch for</param>
        public UsageWatcher(Resolution resolution)
        {
            uService = new UsageService(new UsageStore(resolution));
        }

        public bool IsUsedInGivenResolution(Resolution resolution)
        {
            throw new NotImplementedException();
        }
    }
}
