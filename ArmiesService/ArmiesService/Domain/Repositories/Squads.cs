using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Squads;
using ArmiesService.Common.CachingOperations;
using MongoDB.Bson.Serialization;
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

        private readonly IFactory cachingOperationsFactory;

        public Squads(IFactory cachingOperationsFactory)
        {
            this.cachingOperationsFactory = cachingOperationsFactory;
        }

        public async Task<SquadDto> GetByTypeAsync(string type)
        {
            var searchParams = CreateSearchParamsFromSquadType(type);
            var entityGettingStrategy = cachingOperationsFactory.CreateGetEntityStrategy<SquadDto>(searchParams);
            return await entityGettingStrategy.GetAsync() ?? throw EntityNotFoundException.CreateSquad(type);
        }

        private SearchEntityParams CreateSearchParamsFromSquadType(string squadType) => new SearchEntityParams
        {
            EntityId = squadType,
            CacheKey = GetCacheKeyFromType(squadType),
            CollectionName = "squads"
        };

        private string GetCacheKeyFromType(string type) => $"squad:type:{type}";
    }
}
