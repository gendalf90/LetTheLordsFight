using MapDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapDomain.Entities;
using MongoDB.Driver;
using MapDomain.Common;
using MongoDB.Bson;

namespace MapService.Repositories
{
    public class MapObjectsRepository : IMapObjectsRepository
    {
        private readonly IMongoCollection<MapObjectMongoEntity> collection;

        public MapObjectsRepository(MongoClient client)
        {
            collection = client.GetDatabase("Map").GetCollection<MapObjectMongoEntity>("objects");
        }

        public IEnumerable<MapObject> GetAllMovingObjects()
        {
            throw new NotImplementedException();
        }

        public MapObject GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Save(IEnumerable<MapObject> objects)
        {
            throw new NotImplementedException();
        }

        public void Save(MapObject @object)
        {
            throw new NotImplementedException();
        }
    }

    class MapObjectMongoEntity
    {
        public ObjectId Id { get; set; }

        public void LoadFromRepositoryData(MapObjectRepositoryData repositoryData)
        {
            throw new NotImplementedException();
        }

        public MapObjectRepositoryData ConvertToRepositoryData()
        {
            throw new NotImplementedException();
        }
    }
}
