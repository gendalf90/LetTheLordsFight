using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllSquads
{
    class AllSquadsQuery : IQuery<IEnumerable<SquadQueryDto>>
    {
        static AllSquadsQuery()
        {
            BsonClassMap.RegisterClassMap<SquadQueryDto>(cm =>
            {
                cm.MapIdProperty(e => e.Type);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }

        private readonly IMongoDatabase database;

        public AllSquadsQuery(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<SquadQueryDto>> AskAsync()
        {
            return await database.GetCollection<SquadQueryDto>("squads")
                                 .AsQueryable()
                                 .ToListAsync();
        }
    }
}
