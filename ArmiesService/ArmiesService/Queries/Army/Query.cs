using ArmiesService.Common;
using ArmiesService.Common.CachingOperations;
using MongoDB.Bson.Serialization;
using System.Threading.Tasks;
using ICachingOperationsFactory = ArmiesService.Common.CachingOperations.IFactory;

namespace ArmiesService.Queries.Army
{
    class Query : IQuery<ArmyDto>
    {
        static Query()
        {
            BsonClassMap.RegisterClassMap<ArmyDto>(cm =>
            {
                cm.MapIdProperty(e => e.OwnerLogin);
                cm.MapProperty(e => e.Squads).SetElementName("squads");
            });

            BsonClassMap.RegisterClassMap<SquadDto>(cm =>
            {
                cm.MapProperty(e => e.Armors).SetElementName("armors");
                cm.MapProperty(e => e.Weapons).SetElementName("weapons");
                cm.MapProperty(e => e.Quantity).SetElementName("quantity");
                cm.MapProperty(e => e.Type).SetElementName("type");
            });
        }

        private readonly ICachingOperationsFactory cachingOperationsFactory;
        private readonly IGetCurrentUserLoginStrategy currentUserLogin;

        public Query(ICachingOperationsFactory cachingOperationsFactory, IGetCurrentUserLoginStrategy currentUserLogin)
        {
            this.cachingOperationsFactory = cachingOperationsFactory;
            this.currentUserLogin = currentUserLogin;
        }

        public async Task<ArmyDto> AskAsync()
        {
            var login = currentUserLogin.Get();
            var searchParams = CreateSearchParamsFromOwnerLogin(login);
            var entityGettingStrategy = cachingOperationsFactory.CreateGetEntityStrategy<ArmyDto>(searchParams);
            return await entityGettingStrategy.GetAsync() ?? throw new ArmyNotFoundException(login);
        }

        private SearchEntityParams CreateSearchParamsFromOwnerLogin(string login) => new SearchEntityParams
        {
            EntityId = login,
            CacheKey = GetCacheKeyFromOwnerLogin(login),
            CollectionName = "armies"
        };

        private string GetCacheKeyFromOwnerLogin(string login) => $"army:owner.login:{login}";
    }
}
