using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Squads;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Repositories
{
    class Squads : ISquads
    {
        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;

        public Squads(IDistributedCache cache, IMongoDatabase database)
        {
            this.cache = cache;
            this.database = database;
        }

        public async Task<SquadDto> GetByTypeAsync(string type)
        {
            var collection = database.GetCollection<SquadDto>("squads");
            var bson = await cache.GetAsync(type);

            if(bson != null)
            {
                return BsonSerializer.Deserialize<SquadDto>(bson);
            }

            var squad = await collection.Find(data => data.Type == type).FirstOrDefaultAsync() ?? throw EntityNotFoundException.CreateSquad(type);
            await cache.SetAsync(type, squad.ToBson(), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) });
            return squad;
        }

        public static void RegisterTypes()
        {
            BsonClassMap.RegisterClassMap<SquadDto>(cm =>
            {
                cm.MapIdProperty(e => e.Type);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }
    }
}
