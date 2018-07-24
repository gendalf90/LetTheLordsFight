using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Squads;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Repositories
{
    class Squads : ISquads
    {
        static Squads()
        {
            BsonClassMap.RegisterClassMap<SquadDto>(cm =>
            {
                cm.MapIdProperty(e => e.Type);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }

        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;
        private readonly IOptions<DistributedCacheEntryOptions> cacheOptions;

        public Squads(IDistributedCache cache,
                      IMongoDatabase database,
                      IOptions<DistributedCacheEntryOptions> cacheOptions)
        {
            this.cache = cache;
            this.database = database;
            this.cacheOptions = cacheOptions;
        }

        public async Task<SquadDto> GetByTypeAsync(string type)
        {
            var cacheKey = GetCacheKeyFromType(type);
            var cached = await cache.GetAsync(cacheKey);

            if (cached != null)
            {
                return BsonSerializer.Deserialize<SquadDto>(cached);
            }

            var stored = await Collection.Find(squad => squad.Type == type).FirstOrDefaultAsync() ?? throw EntityNotFoundException.CreateSquad(type);
            await cache.SetAsync(cacheKey, stored.ToBson(), cacheOptions.Value);
            return stored;
        }

        private IMongoCollection<SquadDto> Collection => database.GetCollection<SquadDto>("squads");

        private string GetCacheKeyFromType(string type) => $"squad:type:{type}";
    }
}
