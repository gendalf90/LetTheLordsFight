using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Options
{
    class EventsOptions
    {
        public TimeSpan MakeSnapshotForOlderThan { get; set; }

        public int StartSnapshotMakingLimit { get; set; }
    }
}
