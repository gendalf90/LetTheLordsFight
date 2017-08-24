using MapDomain.Entities;
using MapDomain.Repositories;
using MapDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Commands
{
    class UpdateMapCommand : ICommand
    {
        private readonly IUserValidationService userValidationService;
        private readonly IMapObjectsRepository mapObjectsRepository;

        private TimeSpan elapsed;
        private IEnumerable<MapObject> movingObjects;

        public UpdateMapCommand(IUserValidationService userValidationService, 
                                IMapObjectsRepository mapObjectsRepository,
                                TimeSpan elapsed)
        {
            this.userValidationService = userValidationService;
            this.mapObjectsRepository = mapObjectsRepository;
            this.elapsed = elapsed;
        }

        public async Task ExecuteAsync()
        {
            Validate();
            await LoadMovingObjectsAsync();
            await UpdateMovingObjectsAsync();
            await SaveMovingObjectsAsync();
        }

        private void Validate()
        {
            userValidationService.CurrentCanUpdateMap();
        }

        private async Task LoadMovingObjectsAsync()
        {
            movingObjects = await mapObjectsRepository.GetAllMovingObjectsAsync();
        }

        private async Task UpdateMovingObjectsAsync()
        {
            await Task.WhenAll(movingObjects.Select(ToUpdateMovingTask));
        }

        private Task ToUpdateMovingTask(MapObject obj)
        {
            return Task.Run(() => obj.UpdateMoving(elapsed));
        }

        private async Task SaveMovingObjectsAsync()
        {
            await mapObjectsRepository.SaveLocationAndVisibleAsync(movingObjects);
        }
    }
}
