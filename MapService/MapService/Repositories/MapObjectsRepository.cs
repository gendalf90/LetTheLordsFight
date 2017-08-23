using MapDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapDomain.Entities;
using MapDomain.Common;
using Cassandra.Data.Linq;
using Cassandra;
using MapDomain.Factories;

namespace MapService.Repositories
{
    public class MapObjectsRepository : IMapObjectsRepository
    {
        private readonly ISession session;
        private readonly IMapFactory mapFactory;

        public MapObjectsRepository(ISession session, IMapFactory mapFactory)
        {
            this.session = session;
            this.mapFactory = mapFactory;
        }

        public async Task AddAsync(MapObject obj)
        {
            var data = obj.GetRepositoryData();
            var table = new Table<MapObjectRepositoryData>(session);
            await table.Insert(data).ExecuteAsync();
        }

        public async Task<IEnumerable<MapObject>> GetAllMovingObjectsAsync()
        {
            var table = new Table<MapObjectRepositoryData>(session);
            var map = mapFactory.GetMap();
            var dataset = await table.Where(obj => obj.LocationX != obj.DestinationX || obj.LocationY != obj.DestinationY).ExecuteAsync();
            return dataset.Select(data => new MapObject(data, map));
        }

        public async Task<MapObject> GetByIdAsync(string id)
        {
            var table = new Table<MapObjectRepositoryData>(session);
            var data = await table.First(obj => obj.Id == id).ExecuteAsync();
            var map = mapFactory.GetMap();
            return new MapObject(data, map);
        }

        public async Task SaveDestinationAsync(MapObject mapObj)
        {
            var table = new Table<MapObjectRepositoryData>(session);
            var data = mapObj.GetRepositoryData();
            await table.Where(obj => obj.Id == data.Id)
                       .Select(obj => new MapObjectRepositoryData { DestinationX = data.DestinationX, DestinationY = data.DestinationY })
                       .Update()
                       .ExecuteAsync();
        }

        public async Task SaveLocationAndVisibleAsync(IEnumerable<MapObject> objects)
        {
            var requests = objects.Select(ToSaveLocationAndVisibleRequest);
            await session.CreateBatch()
                         .Append(requests)
                         .ExecuteAsync();
        }

        private CqlUpdate ToSaveLocationAndVisibleRequest(MapObject mapObject)
        {
            var table = new Table<MapObjectRepositoryData>(session);
            var data = mapObject.GetRepositoryData();
            return table.Where(obj => obj.Id == data.Id)
                        .Select(obj => new MapObjectRepositoryData { LocationX = data.LocationX, LocationY = data.LocationY, IsVisible = data.IsVisible })
                        .Update();
        }
    }
}
