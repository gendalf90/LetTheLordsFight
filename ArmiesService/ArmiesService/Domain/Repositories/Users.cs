using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Users;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Repositories
{
    class Users : IUsers
    {
        private readonly IMongoDatabase database;

        public Users(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<UserDto> GetByLoginAsync(string login)
        {
            return await Collection.Find(data => data.Login == login).FirstOrDefaultAsync() ?? throw EntityNotFoundException.CreateUser(login);
        }

        public async Task SaveAsync(UserDto data)
        {
            await Collection.ReplaceOneAsync(user => user.Login == data.Login, data, new UpdateOptions { IsUpsert = true });
        }

        private IMongoCollection<UserDto> Collection => database.GetCollection<UserDto>("users");

        public static void RegisterTypes()
        {
            BsonClassMap.RegisterClassMap<UserDto>(cm =>
            {
                cm.MapIdProperty(e => e.Login);
                cm.MapProperty(e => e.ArmyCostLimit).SetElementName("army_cost_limit");
            });
        }
    }
}
