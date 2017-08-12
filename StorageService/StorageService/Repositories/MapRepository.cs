using StorageDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageDomain.ValueObjects;
using StorageService.Services;
using StorageService.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

namespace StorageService.Repositories
{
    class MapRepository : IMapRepository
    {
        private readonly IMapService mapService;
        private readonly IUsersService usersService;

        public MapRepository(IMapService mapService, IUsersService usersService)
        {
            this.usersService = usersService;
            this.mapService = mapService;
        }

        public async Task<Coordinate> GetStorageCoordinateAsync(string storageId)
        {
            var user = await usersService.GetByStorageIdAsync(storageId);
            var mapObject = await mapService.GetByIdAsync(user.MapObjectId);
            return ToCoordinate(mapObject);
        }

        private Coordinate ToCoordinate(MapObjectDto dto)
        {
            return new Coordinate(dto.X, dto.Y);
        }
    }
}
