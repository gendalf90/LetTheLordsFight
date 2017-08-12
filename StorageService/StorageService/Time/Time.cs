using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Time
{
    class Time : ITime
    {
        public DateTime GetCurrentUtc()
        {
            return DateTime.UtcNow;
        }
    }
}
