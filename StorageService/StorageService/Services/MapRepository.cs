using StorageDomain.Repositories;
using System.Threading.Tasks;
using StorageDomain.ValueObjects;
using StorageService.Options;
using Microsoft.Extensions.Options;
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

        public async Task<Segment> GetSegmentAsync(string id)
        {
            var serviceAuthToken = await securityService.GetServiceTokenAsync();
            var baseUrl = options.Value.BaseUri.AbsoluteUri;
            var mapObject = await new Url(baseUrl).AppendPathSegment($"api/v1/map/objects/{id}")
                                                  .WithOAuthBearerToken(serviceAuthToken)
                                                  .GetJsonAsync<MapObjectDto>();
            return new Segment(mapObject.Segment.I, mapObject.Segment.J);
        }
    }
}
