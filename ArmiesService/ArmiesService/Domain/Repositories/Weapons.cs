using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Weapons;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Repositories
{
    class Weapons : IWeapons
    {
        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;

        public Weapons(IDistributedCache cache, IMongoDatabase database)
        {
            this.cache = cache;
            this.database = database;
        }

        public async Task<WeaponDto> GetByNameAsync(string name)
        {
            var collection = database.GetCollection<WeaponDto>("weapons");
            var bson = await cache.GetAsync(name);

            if (bson != null)
            {
                return BsonSerializer.Deserialize<WeaponDto>(bson);
            }

            var weapon = await collection.Find(data => data.Name == name).FirstOrDefaultAsync() ?? throw EntityNotFoundException.CreateWeapon(name);
            await cache.SetAsync(name, weapon.ToBson(), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) });
            return weapon;
        }

        public static void RegisterTypes()
        {
            BsonClassMap.RegisterClassMap<WeaponDto>(cm =>
            {
                cm.MapIdProperty(e => e.Name);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
                cm.MapProperty(e => e.Offence).SetElementName("offence");
            });

            BsonClassMap.RegisterClassMap<OffenceDto>(cm =>
            {
                cm.MapProperty(e => e.Max).SetElementName("max");
                cm.MapProperty(e => e.Min).SetElementName("min");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }
    }
}
