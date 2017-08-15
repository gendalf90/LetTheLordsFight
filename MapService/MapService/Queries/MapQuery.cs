using MapDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MapDomain.Factories;

namespace MapService.Queries
{
    public class MapQuery : IQuery
    {
        private readonly IMapFactory factory;

        public MapQuery(IMapFactory factory)
        {
            this.factory = factory;
        }

        public Task<string> GetJsonAsync()
        {
            var map = factory.GetMap();
            var result = new { Segments = new { Width = map.SegmentsWidth, Height = map.SegmentsHeight } };
            var json = JsonConvert.SerializeObject(result);
            return Task.FromResult(json);
        }
    }
}
