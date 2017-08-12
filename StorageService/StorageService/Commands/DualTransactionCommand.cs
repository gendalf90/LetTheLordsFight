using StorageDomain.Common;
using StorageDomain.Repositories;
using StorageDomain.Services;
using StorageDomain.ValueObjects;
using StorageService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageService.Events;
using StorageDomain.Entities;
using StorageService.Extensions;
using StorageService.Storages;

namespace StorageService.Commands
{
    class DualTransactionCommand : ICommand
    {
        private IStorageStore storageStore;
        private IEventStore eventStore;
        private ITransactionValidationService transactionValidationService;
        private DualTransactionData data;
        private DualTransaction transaction;
        private Event increaseTransactionEvent;
        private Event decreaseTransactionEvent;
        private Event rollbackEvent;
        private StorageEntity fromStorage;
        private StorageEntity toStorage;

        public DualTransactionCommand(
            IStorageStore storageStore,
            IEventStore eventStore,
            ITransactionValidationService transactionValidationService,
            DualTransactionData data)
        {
            this.storageStore = storageStore;
            this.eventStore = eventStore;
            this.transactionValidationService = transactionValidationService;
            this.data = data;
        }

        private void InitializeTransaction()
        {
            var item = new Item(data.ItemName, data.ItemCount);
            transaction = new DualTransaction(data.FromStorageId, data.ToStorageId, item);
        }

        private void InitializeEvents()
        {
            decreaseTransactionEvent = new SingleTransactionEvent(Guid.NewGuid().ToBase64String(), 
                                                                  SingleTransactionType.Decrease, 
                                                                  data.FromStorageId, 
                                                                  data.ItemName, 
                                                                  data.ItemCount);
            increaseTransactionEvent = new SingleTransactionEvent(Guid.NewGuid().ToBase64String(), 
                                                                  SingleTransactionType.Increase, 
                                                                  data.ToStorageId, 
                                                                  data.ItemName, 
                                                                  data.ItemCount);
            rollbackEvent = new SingleTransactionEvent(Guid.NewGuid().ToBase64String(),
                                                       SingleTransactionType.Increase,
                                                       data.FromStorageId,
                                                       data.ItemName,
                                                       data.ItemCount);
        }

        public async Task ExecuteAsync()
        {
            InitializeTransaction();
            InitializeEvents();
            await ValidateTransactionAsync();
            await SaveDecreaseTransactionAsync();
            await RestoreFromStorageAsync();
            await DoIncreaseTransactionIfDecreaseSuccessOrThrowErrorAsync();
            await RestoreToStorageAsync();
            await RollbackDecreaseTransactionIfIncreaseFailedAndThrowErrorAsync();
        }

        private async Task ValidateTransactionAsync()
        {
            await transactionValidationService.ValidateAsync(transaction);
        }

        private async Task SaveDecreaseTransactionAsync()
        {
            await eventStore.AddAsync(decreaseTransactionEvent);
        }

        private async Task RestoreFromStorageAsync()
        {
            fromStorage = await storageStore.RestoreStorageToThisExclusiveEventIdAsync(decreaseTransactionEvent.Id, transaction.Decrease.StorageId);
        }

        private async Task DoIncreaseTransactionIfDecreaseSuccessOrThrowErrorAsync()
        {
            if (fromStorage.IsTransactionPossible(transaction.Decrease))
            {
                await eventStore.AddAsync(increaseTransactionEvent);
            }
            else
            {
                throw new Exception();
            }
        }

        private async Task RestoreToStorageAsync()
        {
            toStorage = await storageStore.RestoreStorageToThisExclusiveEventIdAsync(increaseTransactionEvent.Id, transaction.Increase.StorageId);
        }

        private async Task RollbackDecreaseTransactionIfIncreaseFailedAndThrowErrorAsync()
        {
            if(toStorage.IsTransactionPossible(transaction.Increase))
            {
                return;
            }

            await eventStore.AddAsync(rollbackEvent);
            throw new Exception();
        }
    }
}
