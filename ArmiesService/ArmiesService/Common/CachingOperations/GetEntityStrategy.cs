using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ArmiesService.Common.CachingOperations
{
    class GetEntityStrategy<T> : IGetEntityStrategy<T> where T: class
    {
        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;
        private readonly IOptions<DistributedCacheEntryOptions> cacheOptions;
        private readonly SearchEntityParams searchParams;

        public GetEntityStrategy(IDistributedCache cache, 
                                 IMongoDatabase database, 
                                 IOptions<DistributedCacheEntryOptions> cacheOptions,
                                 SearchEntityParams searchParams)
        {
            this.cache = cache;
            this.database = database;
            this.cacheOptions = cacheOptions;
            this.searchParams = searchParams;
        }

        public async Task<T> GetAsync()
        {
            var cachedValue = await GetFromCacheAsync();

            if(cachedValue != null)
            {
                return cachedValue;
            }

            var storedValue = await GetFromDatabaseAsync();
            await AddToCacheAsync(storedValue);
            return storedValue;
        }

        private async Task<T> GetFromCacheAsync()
        {
            var bytes = await cache.GetAsync(searchParams.CacheKey);

            if (bytes == null)
            {
                return null;
            }

            return BsonSerializer.Deserialize<T>(bytes);
        }

        private async Task AddToCacheAsync(T entity)
        {
            await cache.SetAsync(searchParams.CacheKey, entity.ToBson(), cacheOptions.Value);
        }

        private async Task<T> GetFromDatabaseAsync()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", searchParams.EntityId);
            var collection = database.GetCollection<BsonDocument>(searchParams.CollectionName);
            var document = await collection.Find(filter).FirstOrDefaultAsync();
            return BsonSerializer.Deserialize<T>(document);
        }
    }
}
