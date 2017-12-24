using StorageDomain.Services;
using StorageService.Storages;
using StorageDto = StorageService.Queries.GetStorage.Storage;
using StorageQuery = StorageService.Queries.GetStorage.Query;

namespace StorageService.Queries
{
    class Factory : IFactory
    {
        private readonly IStorageStore storageStore;
        private readonly IUserValidationService userValidationService;

        public Factory(IStorageStore storageStore, IUserValidationService userValidationService)
        {
            this.storageStore = storageStore;
            this.userValidationService = userValidationService;
        }

        public IQuery<StorageDto> CreateGetStorageQuery(string storageId)
        {
            return new StorageQuery(storageStore, userValidationService, storageId);
        }
    }
}
