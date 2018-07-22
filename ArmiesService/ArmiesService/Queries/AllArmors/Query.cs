using ArmiesService.Common.CachingOperations;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using ICachingOperationsFactory = ArmiesService.Common.CachingOperations.IFactory;

namespace ArmiesService.Queries.AllArmors
{
    class Query : IQuery<IEnumerable<ArmorDto>>
    {
        static Query()
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

        private readonly ICachingOperationsFactory cachingOperationsFactory;

        public Query(ICachingOperationsFactory cachingOperationsFactory)
        {
            this.cachingOperationsFactory = cachingOperationsFactory;
        }

        public async Task<IEnumerable<ArmorDto>> AskAsync()
        {
            var searchParams = CreateSearchParams();
            var entityGettingStrategy = cachingOperationsFactory.CreateGetAllStrategy<ArmorDto>(searchParams);
            return await entityGettingStrategy.GetAsync();
        }

        private SearchAllParams CreateSearchParams() => new SearchAllParams
        {
            CacheKey = "armors:all",
            CollectionName = "armors"
        };
    }
}
