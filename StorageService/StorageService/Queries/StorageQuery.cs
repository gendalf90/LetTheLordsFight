using Newtonsoft.Json;
using StorageDomain.Entities;
using StorageDomain.Services;
using StorageService.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Queries
{
    public class StorageQuery : IQuery
    {
        private readonly IStorageStore storageStore;
        private readonly IUserValidationService userValidationService;
        private readonly string storageId;

        private Storage restoredStorage;

        public StorageQuery(IStorageStore storageStore, IUserValidationService userValidationService, string storageId)
        {
            this.storageStore = storageStore;
            this.userValidationService = userValidationService;
            this.storageId = storageId;
        }

        public async Task<string> AskAsync()
        {
            ValidateQuery();
            await RestoreStorage();
            return GetJson();
        }

        private void ValidateQuery()
        {
            userValidationService.CurrentUserShouldBeOwnerOfThisStorageOrSystem(storageId);
        }

        private async Task RestoreStorage()
        {
            restoredStorage = await storageStore.RestoreStorageToLastEventAsync(storageId);
        }

        private string GetJson()
        {
            var data = restoredStorage.GetRepositoryData();
            var dto = new { Id = data.Id, Items = data.Items.Select(item => new { Name = item.Name, Count = item.Quantity }) };
            return JsonConvert.SerializeObject(dto);
        }
    }
}
