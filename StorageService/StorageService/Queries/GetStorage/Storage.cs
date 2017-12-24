using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Queries.GetStorage
{
    public class Storage
    {
        public string Id { get; set; }

        public Item[] Items { get; set; }
    }
}
