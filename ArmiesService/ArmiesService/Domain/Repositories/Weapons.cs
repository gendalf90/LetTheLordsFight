using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Weapons;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Repositories
{
    class Weapons : IWeapons
    {
        static Weapons()
        {
            BsonClassMap.RegisterClassMap<WeaponRepositoryDto>(cm =>
            {
                cm.MapIdProperty(e => e.Name);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
                cm.MapProperty(e => e.Offence).SetElementName("offence");
            });

            BsonClassMap.RegisterClassMap<OffenceRepositoryDto>(cm =>
            {
                cm.MapProperty(e => e.Max).SetElementName("max");
                cm.MapProperty(e => e.Min).SetElementName("min");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }

        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;
        private readonly IOptions<DistributedCacheEntryOptions> cacheOptions;

        public Weapons(IDistributedCache cache,
                       IMongoDatabase database,
                       IOptions<DistributedCacheEntryOptions> cacheOptions)
        {
            this.cache = cache;
            this.database = database;
            this.cacheOptions = cacheOptions;
        }

        public async Task<WeaponRepositoryDto> GetByNameAsync(string name)
        {
            var cacheKey = GetCacheKeyFromName(name);
            var cached = await cache.GetAsync(cacheKey);

            if (cached != null)
            {
                return BsonSerializer.Deserialize<WeaponRepositoryDto>(cached);
            }

            var stored = await Collection.Find(weapon => weapon.Name == name).FirstOrDefaultAsync() ?? throw EntityNotFoundException.CreateWeapon(name);
            await cache.SetAsync(cacheKey, stored.ToBson(), cacheOptions.Value);
            return stored;
        }

        private IMongoCollection<WeaponRepositoryDto> Collection => database.GetCollection<WeaponRepositoryDto>("weapons");

        private string GetCacheKeyFromName(string name) => $"weapon:name:{name}";
    }
}
