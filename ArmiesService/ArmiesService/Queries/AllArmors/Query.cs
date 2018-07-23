using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllArmors
{
    class Query : IQuery<IEnumerable<ArmorDto>>
    {
        static Query()
        {
            BsonClassMap.RegisterClassMap<ArmorDto>(cm =>
            {
                cm.MapIdProperty(e => e.Name);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
                cm.MapProperty(e => e.Defence).SetElementName("defence");
            });

            BsonClassMap.RegisterClassMap<DefenceDto>(cm =>
            {
                cm.MapProperty(e => e.Max).SetElementName("max");
                cm.MapProperty(e => e.Min).SetElementName("min");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }

        private readonly IMongoDatabase database;

        public Query(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<ArmorDto>> AskAsync()
        {
            return await database.GetCollection<ArmorDto>("armors")
                                 .AsQueryable()
                                 .ToListAsync();
        }
    }
}
