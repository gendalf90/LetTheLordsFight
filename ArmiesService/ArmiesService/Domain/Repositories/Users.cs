using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Users;
using ArmiesService.Common;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
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

        private readonly IGetFromDatabaseWithCachingStrategy getEntityStrategy;
        private readonly IMongoDatabase database;

        public Users(IMongoDatabase database, IGetFromDatabaseWithCachingStrategy getEntityStrategy)
        {
            this.database = database;
            this.getEntityStrategy = getEntityStrategy;
        }

        public async Task<UserDto> GetByLoginAsync(string login)
        {
            var searchParams = CreateSearchParamsFromLogin(login);
            return await getEntityStrategy.GetAsync<UserDto>(searchParams) ?? throw EntityNotFoundException.CreateUser(login);
        }

        public async Task SaveAsync(UserDto data)
        {
            var collection = database.GetCollection<UserDto>(CollectionName);
            await collection.ReplaceOneAsync(user => user.Login == data.Login, data, new UpdateOptions { IsUpsert = true });
        }

        private string CollectionName => "users";

        private SearchParams CreateSearchParamsFromLogin(string login) => new SearchParams
        {
            EntityId = login,
            CacheKey = GetCacheKeyFromLogin(login),
            CollectionName = CollectionName
        };

        private string GetCacheKeyFromLogin(string login) => $"user:login:{login}";
        
    }
}
