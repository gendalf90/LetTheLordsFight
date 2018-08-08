using ArmiesService.Controllers.Data;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllWeapons
{
    class AllWeaponsQuery : IQuery<IEnumerable<WeaponDictionaryDto>>
    {
        static AllWeaponsQuery()
        {
            BsonClassMap.RegisterClassMap<WeaponDictionaryDto>(cm =>
            {
                cm.MapIdProperty(e => e.Name);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
                cm.MapProperty(e => e.Offence).SetElementName("offence");
            });

            BsonClassMap.RegisterClassMap<OffenceDictionaryDto>(cm =>
            {
                cm.MapProperty(e => e.Max).SetElementName("max");
                cm.MapProperty(e => e.Min).SetElementName("min");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }

        private readonly IMongoDatabase database;

        public AllWeaponsQuery(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<WeaponDictionaryDto>> AskAsync()
        {
            return await database.GetCollection<WeaponDictionaryDto>("weapons")
                                 .AsQueryable()
                                 .ToListAsync();
        }
    }
}
