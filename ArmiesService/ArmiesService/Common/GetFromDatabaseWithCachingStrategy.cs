using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace ArmiesService.Common
{
    class GetFromDatabaseWithCachingStrategy : IGetFromDatabaseWithCachingStrategy
    {
        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;
        private readonly IOptions<DistributedCacheEntryOptions> cacheOptions;

        private SearchParams searchParams;

        public GetFromDatabaseWithCachingStrategy(IDistributedCache cache,
                                                  IMongoDatabase database,
                                                  IOptions<DistributedCacheEntryOptions> cacheOptions)
        {
            this.cache = cache;
            this.database = database;
            this.cacheOptions = cacheOptions;
        }

        public async Task<T> GetAsync<T>(SearchParams searchParams) where T : class
        {
            InitializeSearch(searchParams);
            var cachedValue = await GetFromCacheAsync<T>();

            if (cachedValue != null)
            {
                return cachedValue;
            }

            var storedValue = await GetFromDatabaseAsync<T>();
            await AddToCacheAsync(storedValue);
            return storedValue;
        }

        private void InitializeSearch(SearchParams searchParams)
        {
            this.searchParams = searchParams ?? throw new ArgumentNullException(nameof(searchParams));
        }

        private async Task<T> GetFromCacheAsync<T>() where T : class
        {
            var bytes = await cache.GetAsync(searchParams.CacheKey);

            if (bytes == null)
            {
                return null;
            }

            return BsonSerializer.Deserialize<T>(bytes);
        }

        private async Task AddToCacheAsync<T>(T entity) where T : class
        {
            await cache.SetAsync(searchParams.CacheKey, entity.ToBson(), cacheOptions.Value);
        }

        private async Task<T> GetFromDatabaseAsync<T>() where T : class
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", searchParams.EntityId);
            var collection = database.GetCollection<BsonDocument>(searchParams.CollectionName);
            var document = await collection.Find(filter).FirstOrDefaultAsync();
            return BsonSerializer.Deserialize<T>(document);
        }
    }
}
