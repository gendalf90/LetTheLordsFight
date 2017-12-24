using System.Threading.Tasks;
using MapDomain.Factories;
using Newtonsoft.Json.Linq;

namespace MapService.Queries
{
    public class MapQuery : IQuery
    {
        private readonly IMapFactory factory;

        public MapQuery(IMapFactory factory)
        {
            this.factory = factory;
        }

        public Task<JObject> GetJsonAsync()
        {
            var map = factory.GetMap();
            var result = new { Segments = new { Width = map.SegmentsWidth, Height = map.SegmentsHeight } };
            var json = JObject.FromObject(result);
            return Task.FromResult(json);
        }
    }
}
