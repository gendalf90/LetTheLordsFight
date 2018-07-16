using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Armors;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Repositories
{
    class Armors : IArmors
    {
        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;

        public Armors(IDistributedCache cache, IMongoDatabase database)
        {
            this.cache = cache;
            this.database = database;
        }

        public async Task<ArmorDto> GetByNameAsync(string name)
        {
            var collection = database.GetCollection<ArmorDto>("armors");
            var bson = await cache.GetAsync(name);

            if (bson != null)
            {
                return BsonSerializer.Deserialize<ArmorDto>(bson);
            }

            var armor = await collection.Find(data => data.Name == name).FirstOrDefaultAsync() ?? throw EntityNotFoundException.CreateArmor(name);
            await cache.SetAsync(name, armor.ToBson(), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) });
            return armor;
        }

        public static void RegisterTypes()
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
    }
}
