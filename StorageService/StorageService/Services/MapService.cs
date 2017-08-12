using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StorageService.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace StorageService.Services
{
    class MapService : IMapService
    {
        private readonly ISecurityService securityService;
        private readonly string baseUrl;

        public MapService(IOptions<ApiOptions> options, ISecurityService securityService)
        {
            this.securityService = securityService;
            baseUrl = options.Value.BaseUri.AbsoluteUri;
        }

        public async Task<MapObjectDto> GetByIdAsync(string id)
        {
            var serviceAuthToken = await securityService.GetServiceTokenAsync();

            return await new Url(baseUrl).AppendPathSegment($"api/v1/map/objects/{id}")
                                         .WithOAuthBearerToken(serviceAuthToken)
                                         .GetJsonAsync<MapObjectDto>();
        }
    }
}
