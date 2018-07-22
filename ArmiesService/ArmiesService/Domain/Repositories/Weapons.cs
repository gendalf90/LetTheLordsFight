using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Weapons;
using ArmiesService.Common.CachingOperations;
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

        private readonly IFactory cachingOperationsFactory;

        public Weapons(IFactory cachingOperationsFactory)
        {
            this.cachingOperationsFactory = cachingOperationsFactory;
        }

        public async Task<WeaponDto> GetByNameAsync(string name)
        {
            var searchParams = CreateSearchParamsFromWeaponName(name);
            var entityGettingStrategy = cachingOperationsFactory.CreateGetEntityStrategy<WeaponDto>(searchParams);
            return await entityGettingStrategy.GetAsync() ?? throw EntityNotFoundException.CreateWeapon(name);
        }

        private SearchEntityParams CreateSearchParamsFromWeaponName(string armorName) => new SearchEntityParams
        {
            EntityId = armorName,
            CacheKey = GetCacheKeyFromName(armorName),
            CollectionName = "weapons"
        };

        private string GetCacheKeyFromName(string name) => $"weapon:name:{name}";
    }
}
