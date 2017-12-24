using StorageDomain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainItem = StorageDomain.ValueObjects.Item;

namespace StorageService.Queries.GetStorage
{
    public static class Extensions
    {
        public static Storage ToDto(this StorageRepositoryData repositoryData)
        {
            return new Storage
            {
                Id = repositoryData.Id,
                Items = CreateItemsFrom(repositoryData.Items)
            };
        }

        private static Item[] CreateItemsFrom(IEnumerable<DomainItem> items)
        {
            return items.Select(item => new Item { Name = item.Name, Count = item.Quantity }).ToArray();
        }
    }
}
