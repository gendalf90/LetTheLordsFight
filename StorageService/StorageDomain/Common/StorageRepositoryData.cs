using StorageDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageDomain.Common
{
    public class StorageRepositoryData
    {
        public StorageRepositoryData(string id, IEnumerable<Item> items)
        {
            Id = id;
            Items = items;
        }

        public string Id { get; set; }

        public IEnumerable<Item> Items { get; set; }
    }
}
