using StorageDomain.Services;
using StorageService.Storages;
using System.Threading.Tasks;
using StorageEntity = StorageDomain.Entities.Storage;

namespace StorageService.Queries.GetStorage
{
    public class Query : IQuery<Storage>
    {
        private readonly IStorageStore storageStore;
        private readonly IUserValidationService userValidationService;
        private readonly string storageId;

        private StorageEntity restoredStorage;

        public Query(IStorageStore storageStore, IUserValidationService userValidationService, string storageId)
        {
            this.storageStore = storageStore;
            this.userValidationService = userValidationService;
            this.storageId = storageId;
        }

        public async Task<Storage> AskAsync()
        {
            ValidateQuery();
            await RestoreStorage();
            return CreateResult();
        }

        private void ValidateQuery()
        {
            userValidationService.CurrentUserShouldBeOwnerOfThisStorageOrSystem(storageId);
        }

        private async Task RestoreStorage()
        {
            restoredStorage = await storageStore.RestoreStorageToLastEventAsync(storageId);
        }

        private Storage CreateResult()
        {
            var data = restoredStorage.GetRepositoryData();
            return data.ToDto();
        }
    }
}
