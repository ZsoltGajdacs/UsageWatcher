using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsageWatcher
{
    [Serializable]
    public enum Resolution
    {
        HALF_MINUTE = 30000,
        ONE_MINUTE = 60000,
        TWO_MINUTES = 120000,
        THREE_MINUTES = 180000,
        FOUR_MINUTES = 240000,
        FIVE_MINUTES = 3000000,
        TEN_MINUTES = 600000,
        FIFTEEN_MINUTES = 900000,
        THIRTY_MINUTES = 1800000
    }
}
