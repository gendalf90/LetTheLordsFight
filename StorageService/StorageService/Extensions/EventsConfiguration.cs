using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Extensions
{
    public class EventsConfiguration
    {
        public int IsOldSeconds { get; set; }

        public int MakeSnapshotLimit { get; set; }
    }
}
