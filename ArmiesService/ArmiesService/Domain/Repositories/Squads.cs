using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Squads;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Repositories
{
    class Squads : ISquads
    {
        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;

        public Squads(IDistributedCache cache, IMongoDatabase database)
        {
            this.cache = cache;
            this.database = database;
        }

        public async Task<SquadDto> GetByTypeAsync(string type)
        {
            var bson = await cache.GetAsync(type);

            if(bson != null)
            {
                return BsonSerializer.Deserialize<SquadDto>(bson);
            }

            var squad = await Collection.Find(data => data.Type == type).FirstOrDefaultAsync() ?? throw EntityNotFoundException.CreateSquad(type);
            await cache.SetAsync(type, squad.ToBson());
            return squad;
        }

        private IMongoCollection<SquadDto> Collection => database.GetCollection<SquadDto>("squads");
    }
}
