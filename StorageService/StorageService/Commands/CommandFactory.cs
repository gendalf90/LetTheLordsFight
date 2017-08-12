using StorageDomain.Repositories;
using StorageDomain.Services;
using StorageDomain.ValueObjects;
using StorageService.Common;
using StorageService.Events;
using System;
using Microsoft.Extensions.Options;
using StorageService.Options;
using StorageService.Storages;

namespace StorageService.Commands
{
    class CommandFactory : ICommandFactory
    {
        private readonly IStorageStore storageStore;
        private readonly IEventStore eventStore;
        private readonly ITransactionValidationService transactionValidationService;
        private readonly IUserValidationService userValidationService;
        private readonly ISnapshot snapshot;

        public CommandFactory(
            IStorageStore storageStore,
            IEventStore eventStore,
            ITransactionValidationService transactionValidationService,
            ISnapshot snapshot,
            IUserValidationService userValidationService)
        {
            this.storageStore = storageStore;
            this.eventStore = eventStore;
            this.transactionValidationService = transactionValidationService;
            this.snapshot = snapshot;
            this.userValidationService = userValidationService;
        }
        
        public ICommand GetSingleTransactionCommand(SingleTransactionType type, SingleTransactionData data)
        {
            return new SingleTransactionCommand(storageStore, eventStore, transactionValidationService, type, data);
        }

        public ICommand GetCreateSnapshotCommand()
        {
            return new CreateSnapshotCommand(snapshot, userValidationService);
        }

        public ICommand GetCreateStorageCommand(string newStorageId)
        {
            return new CreateStorageCommand(eventStore, userValidationService, newStorageId);
        }

        public ICommand GetDualTransactionCommand(DualTransactionData data)
        {
            return new DualTransactionCommand(storageStore, eventStore, transactionValidationService, data);
        }
    }
}
