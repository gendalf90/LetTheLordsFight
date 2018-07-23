using ArmiesService.Common;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;

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

        private readonly IMongoDatabase database;
        private readonly IGetCurrentUserLoginStrategy currentUserLogin;

        public Query(IMongoDatabase database, IGetCurrentUserLoginStrategy currentUserLogin)
        {
            this.database = database;
            this.currentUserLogin = currentUserLogin;
        }

        public async Task<ArmyDto> AskAsync()
        {
            var login = currentUserLogin.Get();
            var collection = database.GetCollection<ArmyDto>("armies");
            return await collection.Find(army => army.OwnerLogin == login).FirstOrDefaultAsync() ?? throw new ArmyNotFoundException(login);
        }
    }
}
