using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllWeapons
{
    class AllWeaponsQuery : IQuery<IEnumerable<WeaponQueryDto>>
    {
        static AllWeaponsQuery()
        {
            BsonClassMap.RegisterClassMap<WeaponQueryDto>(cm =>
            {
                cm.MapIdProperty(e => e.Name);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
                cm.MapProperty(e => e.Offence).SetElementName("offence");
            });

            BsonClassMap.RegisterClassMap<OffenceQueryDto>(cm =>
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

        public async Task<IEnumerable<WeaponQueryDto>> AskAsync()
        {
            return await database.GetCollection<WeaponQueryDto>("weapons")
                                 .AsQueryable()
                                 .ToListAsync();
        }
    }
}
