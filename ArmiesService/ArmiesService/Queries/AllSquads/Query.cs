using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllSquads
{
    class Query : IQuery<IEnumerable<SquadDto>>
    {
        static Query()
        {
            BsonClassMap.RegisterClassMap<SquadDto>(cm =>
            {
                cm.MapIdProperty(e => e.Type);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }

        private readonly IMongoDatabase database;

        public Query(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<SquadDto>> AskAsync()
        {
            return await database.GetCollection<SquadDto>("squads")
                                 .AsQueryable()
                                 .ToListAsync();
        }
    }
}
