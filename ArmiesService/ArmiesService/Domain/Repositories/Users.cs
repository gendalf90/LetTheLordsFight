using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Users;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Repositories
{
    class Users : IUsers
    {
        static Users()
        {
            BsonClassMap.RegisterClassMap<UserDto>(cm =>
            {
                cm.MapIdProperty(e => e.Login);
                cm.MapProperty(e => e.ArmyCostLimit).SetElementName("army_cost_limit");
            });
        }

        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;
        private readonly IOptions<DistributedCacheEntryOptions> cacheOptions;

        public Users(IDistributedCache cache,
                     IMongoDatabase database,
                     IOptions<DistributedCacheEntryOptions> cacheOptions)
        {
            this.cache = cache;
            this.database = database;
            this.cacheOptions = cacheOptions;
        }

        public async Task<UserDto> GetByLoginAsync(string login)
        {
            var cacheKey = GetCacheKeyFromLogin(login);
            var cached = await cache.GetAsync(cacheKey);

            if (cached != null)
            {
                return BsonSerializer.Deserialize<UserDto>(cached);
            }

            var stored = await Collection.Find(user => user.Login == login).FirstOrDefaultAsync() ?? throw EntityNotFoundException.CreateUser(login);
            await cache.SetAsync(cacheKey, stored.ToBson(), cacheOptions.Value);
            return stored;
        }

        public async Task SaveAsync(UserDto data)
        {
            await Collection.ReplaceOneAsync(user => user.Login == data.Login, data, new UpdateOptions { IsUpsert = true });
        }

        private IMongoCollection<UserDto> Collection => database.GetCollection<UserDto>("users");

        private string GetCacheKeyFromLogin(string login) => $"user:login:{login}";
        
    }
}
