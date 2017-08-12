using Microsoft.Extensions.Options;
using StorageDomain.Entities;
using StorageDomain.Repositories;
using StorageDomain.Services;
using StorageService.Events;
using StorageService.Options;
using StorageService.Services;
using StorageService.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Commands
{
    class CreateSnapshotCommand : ICommand
    {
        private readonly ISnapshot snapshot;
        private readonly IUserValidationService userValidationService;

        public CreateSnapshotCommand(ISnapshot snapshot, IUserValidationService userValidationService)
        {
            this.snapshot = snapshot;
            this.userValidationService = userValidationService;
        }

        public async Task ExecuteAsync()
        {
            await ValidateAsync();
            await CreateToAllAsync();
        }

        private async Task ValidateAsync()
        {
            await userValidationService.CurrentUserShouldBeSystemOrAdminAsync();
        }

        private async Task CreateToAllAsync()
        {
            await snapshot.CreateToAllAsync();
        }
    }
}
