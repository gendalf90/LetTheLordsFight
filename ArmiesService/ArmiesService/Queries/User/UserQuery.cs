using ArmiesService.Common;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ArmiesService.Queries.User
{
    public class UserQuery : IQuery<UserQueryDto>
    {
        static UserQuery()
        {
            BsonClassMap.RegisterClassMap<UserQueryDto>(cm =>
            {
                cm.MapIdProperty(e => e.Login);
                cm.MapProperty(e => e.ArmyCostLimit).SetElementName("army_cost_limit");
            });
        }

        private readonly IMongoDatabase database;
        private readonly IGetCurrentUserLoginStrategy currentUserLogin;

        public UserQuery(IMongoDatabase database, IGetCurrentUserLoginStrategy currentUserLogin)
        {
            this.database = database;
            this.currentUserLogin = currentUserLogin;
        }

        public async Task<UserQueryDto> AskAsync()
        {
            var login = currentUserLogin.Get();
            var collection = database.GetCollection<UserQueryDto>("users");
            return await collection.Find(army => army.Login == login).FirstOrDefaultAsync();
        }
    }
}
