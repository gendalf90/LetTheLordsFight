using StorageDto = StorageService.Queries.GetStorage.Storage;

namespace StorageService.Queries
{
    public interface IFactory
    {
        IQuery<StorageDto> CreateGetStorageQuery(string storageId);
    }
}
