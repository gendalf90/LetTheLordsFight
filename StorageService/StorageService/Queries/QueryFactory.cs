using StorageDomain.Services;
using StorageService.Storages;

namespace StorageService.Queries
{
    class QueryFactory : IQueryFactory
    {
        private readonly IStorageStore storageStore;
        private readonly IUserValidationService userValidationService;

        public QueryFactory(IStorageStore storageStore, IUserValidationService userValidationService)
        {
            this.storageStore = storageStore;
            this.userValidationService = userValidationService;
        }

        public IQuery CreateStorageQuery(string storageId)
        {
            return new StorageQuery(storageStore, userValidationService, storageId);
        }
    }
}
