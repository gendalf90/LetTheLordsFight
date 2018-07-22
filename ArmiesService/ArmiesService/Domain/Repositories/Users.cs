using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Users;
using ArmiesService.Common.CachingOperations;
using MongoDB.Bson.Serialization;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Repositories
{
    class Users : IUsers
    {
        static Users()
        {
            BsonClassMap.RegisterClassMap<UserDto>(cm =>
            {
                cm.MapIdProperty(e => e.Login);
                cm.MapProperty(e => e.ArmyCostLimit).SetElementName("army_cost_limit");
            });
        }

        private readonly IFactory cachingOperationsFactory;

        public Users(IFactory cachingOperationsFactory)
        {
            this.cachingOperationsFactory = cachingOperationsFactory;
        }

        public async Task<UserDto> GetByLoginAsync(string login)
        {
            var searchParams = CreateSearchParamsFromLogin(login);
            var entityGettingStrategy = cachingOperationsFactory.CreateGetEntityStrategy<UserDto>(searchParams);
            return await entityGettingStrategy.GetAsync() ?? throw EntityNotFoundException.CreateUser(login);
        }

        public async Task SaveAsync(UserDto data)
        {
            var searchParams = CreateSearchParamsFromLogin(data.Login);
            var entityInsertingStrategy = cachingOperationsFactory.CreateInsertEntityStrategy<UserDto>(searchParams);
            await entityInsertingStrategy.InsertAsync(data);
        }

        private SearchEntityParams CreateSearchParamsFromLogin(string login) => new SearchEntityParams
        {
            EntityId = login,
            CacheKey = GetCacheKeyFromLogin(login),
            CollectionName = "users"
        };

        private string GetCacheKeyFromLogin(string login) => $"user:login:{login}";
        
    }
}
