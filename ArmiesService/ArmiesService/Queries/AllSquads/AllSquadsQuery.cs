using ArmiesService.Controllers.Data;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllSquads
{
    class AllSquadsQuery : IQuery<IEnumerable<SquadDictionaryDto>>
    {
        static AllSquadsQuery()
        {
            BsonClassMap.RegisterClassMap<SquadDictionaryDto>(cm =>
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

        public async Task<IEnumerable<SquadDictionaryDto>> AskAsync()
        {
            return await database.GetCollection<SquadDictionaryDto>("squads")
                                 .AsQueryable()
                                 .ToListAsync();
        }
    }
}
