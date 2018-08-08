using ArmiesService.Controllers.Data;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllArmors
{
    class AllArmorsQuery : IQuery<IEnumerable<ArmorDictionaryDto>>
    {
        static AllArmorsQuery()
        {
            BsonClassMap.RegisterClassMap<ArmorDictionaryDto>(cm =>
            {
                cm.MapIdProperty(e => e.Name);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
                cm.MapProperty(e => e.Defence).SetElementName("defence");
            });

            BsonClassMap.RegisterClassMap<DefenceDictionaryDto>(cm =>
            {
                cm.MapProperty(e => e.Max).SetElementName("max");
                cm.MapProperty(e => e.Min).SetElementName("min");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }

        private readonly IMongoDatabase database;

        public AllArmorsQuery(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<ArmorDictionaryDto>> AskAsync()
        {
            return await database.GetCollection<ArmorDictionaryDto>("armors")
                                 .AsQueryable()
                                 .ToListAsync();
        }
    }
}
