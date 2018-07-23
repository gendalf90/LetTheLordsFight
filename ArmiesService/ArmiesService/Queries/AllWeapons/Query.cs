using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllWeapons
{
    class Query : IQuery<IEnumerable<WeaponDto>>
    {
        static Query()
        {
            BsonClassMap.RegisterClassMap<WeaponDto>(cm =>
            {
                cm.MapIdProperty(e => e.Name);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
                cm.MapProperty(e => e.Offence).SetElementName("offence");
            });

            BsonClassMap.RegisterClassMap<OffenceDto>(cm =>
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

        public async Task<IEnumerable<WeaponDto>> AskAsync()
        {
            return await database.GetCollection<WeaponDto>("weapons")
                                 .AsQueryable()
                                 .ToListAsync();
        }
    }
}
