using MapDomain.Entities;
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
    class UpdateObjectCommand : ICommand
    {
        private readonly IUserValidationService userValidationService;
        private readonly IMapObjectsRepository mapObjectsRepository;

        private string id;
        private MapObjectUpdateData updateData;
        private MapObject mapObject;

        public UpdateObjectCommand(IUserValidationService userValidationService,
                                   IMapObjectsRepository mapObjectsRepository,
                                   MapObjectUpdateData updateData,
                                   string id)
        {
            this.userValidationService = userValidationService;
            this.mapObjectsRepository = mapObjectsRepository;
            this.updateData = updateData;
            this.id = id;
        }

        public async Task ExecuteAsync()
        {
            Validate();
            await LoadMapObjectAsync();
            UpdateMapObjectDestination();
            await SaveMapObjectAsync();
        }

        private void Validate()
        {
            userValidationService.CurrentCanChangeDestinationForThisMapObject(id);
        }

        private async Task LoadMapObjectAsync()
        {
            mapObject = await mapObjectsRepository.GetByIdAsync(id);
        }

        private void UpdateMapObjectDestination()
        {
            var destination = ToLocation(updateData.Destination);
            mapObject.GoTo(destination);
        }

        private Location ToLocation(MapPosition position)
        {
            return new Location(position.X.GetValueOrDefault(), position.Y.GetValueOrDefault());
        }

        private async Task SaveMapObjectAsync()
        {
            await mapObjectsRepository.SaveDestinationAsync(mapObject);
        }
    }
}
