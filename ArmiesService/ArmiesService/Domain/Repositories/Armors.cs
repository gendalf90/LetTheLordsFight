using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Armors;
using ArmiesService.Common.CachingOperations;
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

        private readonly IFactory cachingOperationsFactory;

        public Armors(IFactory cachingOperationsFactory)
        {
            this.cachingOperationsFactory = cachingOperationsFactory;
        }

        public async Task<ArmorDto> GetByNameAsync(string name)
        {
            var searchParams = CreateSearchParamsFromArmorName(name);
            var entityGettingStrategy = cachingOperationsFactory.CreateGetEntityStrategy<ArmorDto>(searchParams);
            return await entityGettingStrategy.GetAsync() ?? throw EntityNotFoundException.CreateArmor(name);
        }

        private SearchEntityParams CreateSearchParamsFromArmorName(string armorName) => new SearchEntityParams
        {
            EntityId = armorName,
            CacheKey = GetCacheKeyFromName(armorName),
            CollectionName = "armors"
        };

        private string GetCacheKeyFromName(string name) => $"armor:name:{name}";
    }
}
