using StorageDomain.Common;
using StorageDomain.Repositories;
using StorageDomain.Services;
using StorageDomain.ValueObjects;
using StorageService.Common;
using System;
using StorageDomain.Entities;
using StorageService.Events;
using System.Collections.Generic;
using StorageService.Extensions;
using System.Linq;
using System.Threading.Tasks;
using StorageService.Storages;

namespace StorageService.Commands
{
    class SingleTransactionCommand : ICommand
    {
        private IStorageStore storageStore;
        private IEventStore eventStore;
        private ITransactionValidationService transactionValidationService;
        private SingleTransaction transaction;
        private Event transactionEvent;
        private Storage storage;

        public SingleTransactionCommand(
            IStorageStore storageStore, 
            IEventStore eventStore, 
            ITransactionValidationService transactionValidationService,
            SingleTransactionType type,
            SingleTransactionData data) 
        {
            this.storageStore = storageStore;
            this.eventStore = eventStore;
            this.transactionValidationService = transactionValidationService;
            this.transaction = CreateTransaction(type, data);
            this.transactionEvent = CreateEvent(type, data);
        }

        private SingleTransaction CreateTransaction(SingleTransactionType type, SingleTransactionData data)
        {
            var item = new Item(data.ItemName, data.ItemCount.Value);
            return new SingleTransaction(data.StorageId, type, item);
        }

        private Event CreateEvent(SingleTransactionType type, SingleTransactionData data)
        {
            return new SingleTransactionEvent(Guid.NewGuid().ToBase64String(), type, data.StorageId, data.ItemName, data.ItemCount.Value);
        }

        public async Task ExecuteAsync()
        {
            await ValidateTransactionAsync();
            await SaveTransactionAsync();
            await RestoreStorageAsync();
            TryApplyTransaction();
        }

        private async Task ValidateTransactionAsync()
        {
            await transactionValidationService.ValidateAsync(transaction);
        }

        private async Task SaveTransactionAsync()
        {
            await eventStore.AddAsync(transactionEvent);
        }

        private async Task RestoreStorageAsync()
        {
            storage = await storageStore.RestoreStorageToThisExclusiveEventIdAsync(transactionEvent.Id, transaction.StorageId);
        }

        private void TryApplyTransaction()
        {
            storage.ApplyTransaction(transaction);
        }
    }
}
