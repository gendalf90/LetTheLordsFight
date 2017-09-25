using MapDomain.Common;
using MapDomain.Entities;
using MapDomain.Factories;
using MapDomain.Repositories;
using MapDomain.Services;
using MapDomain.ValueObjects;
using MapService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Commands
{
    class AddObjectCommand : ICommand
    {
        private readonly IUserValidationService userValidationService;
        private readonly IMapObjectsRepository mapObjectsRepository;
        private readonly IMapFactory mapFactory;

        private string id;
        private MapObjectCreateData createData;
        private MapObject mapObject;

        public AddObjectCommand(IUserValidationService userValidationService, 
                                IMapObjectsRepository mapObjectsRepository,
                                IMapFactory mapFactory,
                                MapObjectCreateData createData,
                                string id)
        {
            this.userValidationService = userValidationService;
            this.mapObjectsRepository = mapObjectsRepository;
            this.mapFactory = mapFactory;
            this.id = id;
            this.createData = createData;
        }
        
        public async Task ExecuteAsync()
        {
            Validate();
            CreateMapObject();
            SetMapObjectPosition();
            await SaveMapObjectAsync();
        }

        private void Validate()
        {
            userValidationService.CurrentCanCreateMapObject();
        }

        private void CreateMapObject()
        {
            var map = mapFactory.GetMap();
            mapObject = new MapObject(id, map);
        }

        private void SetMapObjectPosition()
        {
            var location = ToLocation(createData.Location);
            mapObject.SetPosition(location);
        }

        private Location ToLocation(MapPosition position)
        {
            return new Location(position.X.Value, position.Y.Value);
        }

        private async Task SaveMapObjectAsync()
        {
            await mapObjectsRepository.AddAsync(mapObject);
        }
    }
}
