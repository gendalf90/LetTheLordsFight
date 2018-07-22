using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ArmiesService.Common.CachingOperations
{
    class InsertEntityStrategy<T> : IInsertEntityStrategy<T> where T : class
    {
        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;
        private readonly IOptions<DistributedCacheEntryOptions> cacheOptions;
        private readonly SearchEntityParams searchParams;

        public InsertEntityStrategy(IDistributedCache cache,
                                    IMongoDatabase database,
                                    IOptions<DistributedCacheEntryOptions> cacheOptions,
                                    SearchEntityParams searchParams)
        {
            this.cache = cache;
            this.database = database;
            this.cacheOptions = cacheOptions;
            this.searchParams = searchParams;
        }

        public async Task InsertAsync(T entity)
        {
            await InsertToDatabaseAsync(entity);
            await AddToCacheAsync(entity);
        }

        private async Task InsertToDatabaseAsync(T entity)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", searchParams.EntityId);
            var collection = database.GetCollection<BsonDocument>(searchParams.CollectionName);
            await collection.ReplaceOneAsync(filter, entity.ToBsonDocument(), new UpdateOptions { IsUpsert = true });
        }

        private async Task AddToCacheAsync(T entity)
        {
            await cache.SetStringAsync(searchParams.CacheKey, entity.ToJson(), cacheOptions.Value);
        }
    }
}
