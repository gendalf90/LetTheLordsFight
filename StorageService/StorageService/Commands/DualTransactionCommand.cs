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
using StorageDomain.Exceptions;

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
        private Storage fromStorage;
        private Storage toStorage;

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
            var item = new Item(data.ItemName, data.ItemCount.Value);
            transaction = new DualTransaction(data.FromStorageId, data.ToStorageId, item);
        }

        private void InitializeEvents()
        {
            decreaseTransactionEvent = new SingleTransactionEvent(Guid.NewGuid().ToBase64String(), 
                                                                  SingleTransactionType.Decrease, 
                                                                  data.FromStorageId, 
                                                                  data.ItemName, 
                                                                  data.ItemCount.Value);
            increaseTransactionEvent = new SingleTransactionEvent(Guid.NewGuid().ToBase64String(), 
                                                                  SingleTransactionType.Increase, 
                                                                  data.ToStorageId, 
                                                                  data.ItemName, 
                                                                  data.ItemCount.Value);
            rollbackEvent = new SingleTransactionEvent(Guid.NewGuid().ToBase64String(),
                                                       SingleTransactionType.Increase,
                                                       data.FromStorageId,
                                                       data.ItemName,
                                                       data.ItemCount.Value);
        }

        public async Task ExecuteAsync()
        {
            InitializeTransaction();
            InitializeEvents();
            await ValidateTransactionAsync();
            await SaveDecreaseTransactionAsync();
            await RestoreFromStorageAsync();
            await DoIncreaseTransactionIfDecreaseSuccessAsync();
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

        private async Task DoIncreaseTransactionIfDecreaseSuccessAsync()
        {
            fromStorage.ApplyTransaction(transaction.Decrease);
            await eventStore.AddAsync(increaseTransactionEvent);
        }

        private async Task RestoreToStorageAsync()
        {
            toStorage = await storageStore.RestoreStorageToThisExclusiveEventIdAsync(increaseTransactionEvent.Id, transaction.Increase.StorageId);
        }

        private async Task RollbackDecreaseTransactionIfIncreaseFailedAndThrowErrorAsync()
        {
            try
            {
                toStorage.ApplyTransaction(transaction.Increase);
            }
            catch(ValidationException)
            {
                await eventStore.AddAsync(rollbackEvent);
                throw;
            }
        }
    }
}
