using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Weapons;
using ArmiesService.Common;
using MongoDB.Bson.Serialization;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Repositories
{
    class Weapons : IWeapons
    {
        static Weapons()
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

        private readonly IGetFromDatabaseWithCachingStrategy getEntityStrategy;

        public Weapons(IGetFromDatabaseWithCachingStrategy getEntityStrategy)
        {
            this.getEntityStrategy = getEntityStrategy;
        }

        public async Task<WeaponDto> GetByNameAsync(string name)
        {
            var searchParams = CreateSearchParamsFromWeaponName(name);
            return await getEntityStrategy.GetAsync<WeaponDto>(searchParams) ?? throw EntityNotFoundException.CreateWeapon(name);
        }

        private SearchParams CreateSearchParamsFromWeaponName(string armorName) => new SearchParams
        {
            EntityId = armorName,
            CacheKey = GetCacheKeyFromName(armorName),
            CollectionName = "weapons"
        };

        private string GetCacheKeyFromName(string name) => $"weapon:name:{name}";
    }
}
