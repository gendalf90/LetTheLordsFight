using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Squads;
using ArmiesService.Common;
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

        private readonly IGetFromDatabaseWithCachingStrategy getEntityStrategy;

        public Squads(IGetFromDatabaseWithCachingStrategy getEntityStrategy)
        {
            this.getEntityStrategy = getEntityStrategy;
        }

        public async Task<SquadDto> GetByTypeAsync(string type)
        {
            var searchParams = CreateSearchParamsFromSquadType(type);
            return await getEntityStrategy.GetAsync<SquadDto>(searchParams) ?? throw EntityNotFoundException.CreateSquad(type);
        }

        private SearchParams CreateSearchParamsFromSquadType(string squadType) => new SearchParams
        {
            EntityId = squadType,
            CacheKey = GetCacheKeyFromType(squadType),
            CollectionName = "squads"
        };

        private string GetCacheKeyFromType(string type) => $"squad:type:{type}";
    }
}
