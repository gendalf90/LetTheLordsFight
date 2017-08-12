using System;
using System.Threading.Tasks;
using StorageService.Events;
using StorageDomain.Services;
using StorageService.Extensions;
using StorageDomain.Entities;

namespace StorageService.Commands
{
    class CreateStorageCommand : ICommand
    {
        private readonly IEventStore eventStore;
        private readonly IUserValidationService userValidationService;
        private readonly CreateEvent createStorageEvent;

        public CreateStorageCommand(
            IEventStore eventStore,
            IUserValidationService userValidationService,
            string newStorageId)
        {
            this.eventStore = eventStore;
            this.userValidationService = userValidationService;
            this.createStorageEvent = CreateEvent(newStorageId);
        }

        private CreateEvent CreateEvent(string newStorageId)
        {
            return new CreateEvent(Guid.NewGuid().ToBase64String(), newStorageId);
        }
        
        public async Task ExecuteAsync()
        {
            await ValidateAsync();
            await SaveTransactionAsync();
        }

        private async Task ValidateAsync()
        {
            await userValidationService.CurrentUserShouldBeSystemOrAdminAsync();
        }

        private async Task SaveTransactionAsync()
        {
            await eventStore.AddAsync(createStorageEvent);
        }
    }
}
