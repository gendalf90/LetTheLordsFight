using StorageDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageDomain.Common
{
    public class StorageRepositoryData
    {
        public string Id { get; set; }

        public IEnumerable<Item> Items { get; set; }
    }
}
