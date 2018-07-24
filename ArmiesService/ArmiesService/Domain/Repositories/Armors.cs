using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Armors;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;


namespace ArmiesService.Domain.Repositories
{
    class Armors : IArmors
    {
        static Armors()
        {
            BsonClassMap.RegisterClassMap<ArmorDto>(cm =>
            {
                cm.MapIdProperty(e => e.Name);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
                cm.MapProperty(e => e.Defence).SetElementName("defence");
            });

            BsonClassMap.RegisterClassMap<DefenceDto>(cm =>
            {
                cm.MapProperty(e => e.Max).SetElementName("max");
                cm.MapProperty(e => e.Min).SetElementName("min");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }

        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;
        private readonly IOptions<DistributedCacheEntryOptions> cacheOptions;

        public Armors(IDistributedCache cache,
                      IMongoDatabase database,
                      IOptions<DistributedCacheEntryOptions> cacheOptions)
        {
            this.cache = cache;
            this.database = database;
            this.cacheOptions = cacheOptions;
        }

        public async Task<ArmorDto> GetByNameAsync(string name)
        {
            var cacheKey = GetCacheKeyFromName(name);
            var cached = await cache.GetAsync(cacheKey);

            if (cached != null)
            {
                return BsonSerializer.Deserialize<ArmorDto>(cached);
            }

            var stored = await Collection.Find(armor => armor.Name == name).FirstOrDefaultAsync() ?? throw EntityNotFoundException.CreateArmor(name);
            await cache.SetAsync(cacheKey, stored.ToBson(), cacheOptions.Value);
            return stored;
        }

        private IMongoCollection<ArmorDto> Collection => database.GetCollection<ArmorDto>("armors");

        private string GetCacheKeyFromName(string name) => $"armor:name:{name}";
    }
}
