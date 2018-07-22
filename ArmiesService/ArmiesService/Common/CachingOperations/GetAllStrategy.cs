using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmiesService.Common.CachingOperations
{
    class GetAllStrategy<T> : IGetAllStrategy<T> where T : class
    {
        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;
        private readonly IOptions<DistributedCacheEntryOptions> cacheOptions;
        private readonly SearchAllParams searchParams;

        public GetAllStrategy(IDistributedCache cache,
                              IMongoDatabase database,
                              IOptions<DistributedCacheEntryOptions> cacheOptions,
                              SearchAllParams searchParams)
        {
            this.cache = cache;
            this.database = database;
            this.cacheOptions = cacheOptions;
            this.searchParams = searchParams;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            var cached = await GetFromCacheAsync();

            if (cached != null)
            {
                return cached;
            }

            var list = await LoadFromDatabaseAsync();
            await AddToCacheAsync(list);
            return list;
        }

        private async Task<IEnumerable<T>> GetFromCacheAsync()
        {
            var json = await cache.GetStringAsync(searchParams.CacheKey);

            if (json == null)
            {
                return null;
            }

            return BsonSerializer.Deserialize<List<T>>(json);
        }

        private async Task AddToCacheAsync(IEnumerable<T> list)
        {
            await cache.SetStringAsync(searchParams.CacheKey, list.ToJson(), cacheOptions.Value);
        }

        private async Task<IEnumerable<T>> LoadFromDatabaseAsync()
        {
            var collection = database.GetCollection<T>(searchParams.CollectionName);
            return await collection.AsQueryable().ToListAsync();
        }
    }
}
