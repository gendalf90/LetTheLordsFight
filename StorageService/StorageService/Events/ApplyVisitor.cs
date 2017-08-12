using StorageDomain.Common;
using StorageDomain.Entities;
using StorageDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class ApplyVisitor : IEventVisitor
    {
        private ICollection<StorageEntity> source;

        public ApplyVisitor(ICollection<StorageEntity> source)  //меняем состояние сущностей и все, ничего не возвращаем
        {
            this.source = source;
        }

        public async Task VisitAsync(SingleTransactionEvent e)
        {
            var storage = source.FirstOrDefault(entity => entity.Id == e.StorageId);

            if(storage == null)
            {
                return;
            }

            var item = new Item(e.ItemName, e.ItemCount);
            var transaction = new SingleTransaction(e.StorageId, e.TransactionType, item);

            if (storage.IsTransactionPossible(transaction))
            {
                storage.ApplyTransaction(transaction);
            }

            await Task.CompletedTask;
        }

        public async Task VisitAsync(CreateEvent e)
        {
            if(source.Any(entity => entity.Id == e.StorageId))
            {
                return;
            }

            source.Add(new StorageEntity(e.StorageId));
            await Task.CompletedTask;
        }

        public async Task VisitAsync(SnapshotEvent e)
        {
            var storage = source.FirstOrDefault(entity => entity.Id == e.StorageId);

            if (storage != null)
            {
                source.Remove(storage);
            }

            var items = e.Items.Select(pair => new Item(pair.Key, pair.Value));
            var storageData = new StorageRepositoryData(e.StorageId, items);
            source.Add(new StorageEntity(storageData));
            await Task.CompletedTask;
        }
    }
}
