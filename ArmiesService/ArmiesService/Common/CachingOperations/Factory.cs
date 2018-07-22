using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ArmiesService.Common.CachingOperations
{
    class Factory : IFactory
    {
        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;
        private readonly IOptions<DistributedCacheEntryOptions> cacheOptions;

        public Factory(IDistributedCache cache, IMongoDatabase database, IOptions<DistributedCacheEntryOptions> cacheOptions)
        {
            this.cache = cache;
            this.database = database;
            this.cacheOptions = cacheOptions;
        }

        public IGetAllStrategy<T> CreateGetAllStrategy<T>(SearchAllParams searchParams) where T : class
        {
            return new GetAllStrategy<T>(cache, database, cacheOptions, searchParams);
        }

        public IGetEntityStrategy<T> CreateGetEntityStrategy<T>(SearchEntityParams searchParams) where T : class
        {
            return new GetEntityStrategy<T>(cache, database, cacheOptions, searchParams);
        }

        public IInsertEntityStrategy<T> CreateInsertEntityStrategy<T>(SearchEntityParams searchParams) where T : class
        {
            return new InsertEntityStrategy<T>(cache, database, cacheOptions, searchParams);
        }
    }
}
