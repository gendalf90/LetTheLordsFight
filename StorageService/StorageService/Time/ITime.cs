using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Time
{
    interface ITime
    {
        DateTime GetCurrentUtc();
    }
}
