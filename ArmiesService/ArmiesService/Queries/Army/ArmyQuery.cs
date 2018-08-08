using ArmiesService.Common;
using ArmiesService.Controllers.Data;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ArmiesService.Queries.Army
{
    class ArmyQuery : IQuery<ArmyGetDto>
    {
        static ArmyQuery()
        {
            BsonClassMap.RegisterClassMap<ArmyGetDto>(cm =>
            {
                cm.MapIdProperty(e => e.OwnerLogin);
                cm.MapProperty(e => e.Squads).SetElementName("squads");
            });

            BsonClassMap.RegisterClassMap<SquadGetDto>(cm =>
            {
                cm.MapProperty(e => e.Armors).SetElementName("armors");
                cm.MapProperty(e => e.Weapons).SetElementName("weapons");
                cm.MapProperty(e => e.Quantity).SetElementName("quantity");
                cm.MapProperty(e => e.Type).SetElementName("type");
            });
        }

        private readonly IMongoDatabase database;
        private readonly IGetCurrentUserLoginStrategy currentUserLogin;

        public ArmyQuery(IMongoDatabase database, IGetCurrentUserLoginStrategy currentUserLogin)
        {
            this.database = database;
            this.currentUserLogin = currentUserLogin;
        }

        public async Task<ArmyGetDto> AskAsync()
        {
            var login = currentUserLogin.Get();
            var collection = database.GetCollection<ArmyGetDto>("armies");
            return await collection.Find(army => army.OwnerLogin == login).FirstOrDefaultAsync();
        }
    }
}
