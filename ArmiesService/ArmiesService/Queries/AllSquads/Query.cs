using ArmiesService.Common.CachingOperations;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using ICachingOperationsFactory = ArmiesService.Common.CachingOperations.IFactory;

namespace ArmiesService.Queries.AllSquads
{
    class Query : IQuery<IEnumerable<SquadDto>>
    {
        static Query()
        {
            BsonClassMap.RegisterClassMap<SquadDto>(cm =>
            {
                cm.MapIdProperty(e => e.Type);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }

        private readonly ICachingOperationsFactory cachingOperationsFactory;

        public Query(ICachingOperationsFactory cachingOperationsFactory)
        {
            this.cachingOperationsFactory = cachingOperationsFactory;
        }

        public async Task<IEnumerable<SquadDto>> AskAsync()
        {
            var searchParams = CreateSearchParams();
            var entityGettingStrategy = cachingOperationsFactory.CreateGetAllStrategy<SquadDto>(searchParams);
            return await entityGettingStrategy.GetAsync();
        }

        private SearchAllParams CreateSearchParams() => new SearchAllParams
        {
            CacheKey = "squads:all",
            CollectionName = "squads"
        };
    }
}
