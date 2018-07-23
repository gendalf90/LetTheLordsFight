using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Armors;
using ArmiesService.Common;
using MongoDB.Bson.Serialization;
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

        private readonly IGetFromDatabaseWithCachingStrategy getEntityStrategy;

        public Armors(IGetFromDatabaseWithCachingStrategy getEntityStrategy)
        {
            this.getEntityStrategy = getEntityStrategy;
        }

        public async Task<ArmorDto> GetByNameAsync(string name)
        {
            var searchParams = CreateSearchParamsFromArmorName(name);
            return await getEntityStrategy.GetAsync<ArmorDto>(searchParams) ?? throw EntityNotFoundException.CreateArmor(name);
        }

        private SearchParams CreateSearchParamsFromArmorName(string armorName) => new SearchParams
        {
            EntityId = armorName,
            CacheKey = GetCacheKeyFromName(armorName),
            CollectionName = "armors"
        };

        private string GetCacheKeyFromName(string name) => $"armor:name:{name}";
    }
}
