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
using Flurl;
using Flurl.Http;

namespace StorageService.Services
{
    class MapRepository : IMapRepository
    {
        private readonly ISecurityService securityService;
        private readonly IOptions<ApiOptions> options;

        public MapRepository(ISecurityService securityService, IOptions<ApiOptions> options)
        {
            this.securityService = securityService;
            this.options = options;
        }

        public async Task<Coordinate> GetCoordinateAsync(string id)
        {
            var serviceAuthToken = await securityService.GetServiceTokenAsync();
            var baseUrl = options.Value.BaseUri.AbsoluteUri;
            var mapObject = await new Url(baseUrl).AppendPathSegment($"api/v1/map/objects/{id}")
                                                  .WithOAuthBearerToken(serviceAuthToken)
                                                  .GetJsonAsync<MapObjectDto>();
            return new Coordinate(mapObject.X, mapObject.Y);
        }
    }
}
