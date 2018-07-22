using ArmiesDomain.Repositories.Armies;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Repositories
{
    class Armies : IArmies
    {
        static Armies()
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

        public Armies(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task SaveAsync(ArmyDto data)
        {
            var collection = database.GetCollection<ArmyDto>("armies");
            await collection.ReplaceOneAsync(army => army.OwnerLogin == data.OwnerLogin, data, new UpdateOptions { IsUpsert = true });
        }
    }
}
