using MapDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapDomain.Entities;
using MapDomain.Common;
using MapDomain.Factories;
using MongoDB.Driver;

namespace MapService.Repositories
{
    public class MapObjectsRepository : IMapObjectsRepository
    {
        private readonly IMongoCollection<MapObjectModel> objects;
        private readonly IMapFactory mapFactory;

        public MapObjectsRepository(IMongoDatabase mapDatabase, IMapFactory mapFactory)
        {
            this.objects = mapDatabase.GetCollection<MapObjectModel>("objects");
            this.mapFactory = mapFactory;
        }

        public async Task AddAsync(MapObject obj)
        {
            var data = ToModel(obj);
            await objects.InsertOneAsync(data);
        }

        public async Task<IEnumerable<MapObject>> GetAllMovingObjectsAsync()
        {
            var map = mapFactory.GetMap();
            var models = await objects.Find(data => data.IsMoving).ToListAsync();
            return models.Select(model => new MapObject(model, map)).ToList();
        }

        public async Task<MapObject> GetByIdAsync(string id)
        {
            var map = mapFactory.GetMap();
            var model = await objects.Find(data => data.Id == id).FirstAsync();
            return new MapObject(model, map);
        }

        public async Task UpdateDestinationAsync(MapObject mapObj)
        {
            var data = ToModel(mapObj);
            var filter = Filter.Where(model => model.Id == data.Id);
            var update = Update.Combine(Update.Set(model => model.DestinationX, data.DestinationX),
                                        Update.Set(model => model.DestinationY, data.DestinationY));

            if(data.IsMoving)
            {
                update = Update.Combine(update, Update.Set(model => model.IsMoving, true));
            }

            await objects.UpdateOneAsync(filter, update);
        }

        public async Task UpdateLocationAndVisibleAsync(IEnumerable<MapObject> mapObjects)
        {
            var requests = mapObjects.SelectMany(ToUpdateLocationAndVisibleRequests).ToList();
            var options = new BulkWriteOptions { IsOrdered = false };
            await objects.BulkWriteAsync(requests, options);
        }

        private IEnumerable<WriteModel<MapObjectModel>> ToUpdateLocationAndVisibleRequests(MapObject mapObject)
        {
            var data = ToModel(mapObject);
            var filter = Filter.Where(model => model.Id == data.Id);
            var update = Update.Combine(Update.Set(model => model.LocationX, data.LocationX),
                                        Update.Set(model => model.LocationY, data.LocationY),
                                        Update.Set(model => model.IsVisible, data.IsVisible));
            yield return new UpdateOneModel<MapObjectModel>(filter, update);

            if(data.IsMoving)
            {
                yield break;
            }

            filter = Filter.And(filter, Filter.Where(model => model.DestinationX == data.DestinationX && model.DestinationY == data.DestinationY));
            update = Update.Set(model => model.IsMoving, false);
            yield return new UpdateOneModel<MapObjectModel>(filter, update);
        }

        private UpdateDefinitionBuilder<MapObjectModel> Update
        {
            get => Builders<MapObjectModel>.Update;
        }

        private FilterDefinitionBuilder<MapObjectModel> Filter
        {
            get => Builders<MapObjectModel>.Filter;
        }

        private MapObjectModel ToModel(MapObject mapObject)
        {
            var model = new MapObjectModel
            {
                IsMoving = mapObject.IsMoving,
                IsVisible = mapObject.IsVisible
            };
            mapObject.FillRepositoryData(model);
            return model;
        }
    }
}
