using MapDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapService.Queries
{
    public class MapQuery : IQuery
    {
        private readonly IMapRepository repository;

        public MapQuery(IMapRepository repository)
        {
            this.repository = repository;
        }

        public Task<string> GetJsonAsync()
        {
            var map = repository.GetMap();
            var result = new { Segments = new { Width = map.SegmentsWidth, Height = map.SegmentsHeight } };
            var json = JsonConvert.SerializeObject(result);
            return Task.FromResult(json);
        }
    }
}
