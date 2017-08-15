using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapDomain.ValueObjects;
using Microsoft.Extensions.Options;
using MapService.Options;
using MapDomain.Common;
using System.Threading;
using MapDomain.Factories;

namespace MapService.Factories
{
    public class MapFactory : IMapFactory
    {
        private readonly Lazy<Map> map;

        public MapFactory(IOptions<MapOptions> options)
        {
            map = new Lazy<Map>(() => new Map(GetMapCreatingData(options.Value)), LazyThreadSafetyMode.PublicationOnly);
        }

        private MapCreateData GetMapCreatingData(MapOptions options)
        {
            return new MapCreateData
            {
                Types = CreateSegmentTypes(options.Segments),
                SegmentSize = options.Segments.Size,
                SegmentsSpeed = CreateSegmentsSpeed(options.Segments)
            };
        }

        private SegmentType[,] CreateSegmentTypes(Segments segments)
        {
            var result = new SegmentType[segments.Height, segments.Width];

            for(int i = 0, k = 0; i < result.GetLength(0); i++)
            {
                for(int j = 0; j < result.GetLength(1); j++, k++)
                {
                    result[i, j] = (SegmentType)segments.Types[k];
                }
            }

            return result;
        }

        private Dictionary<SegmentType, float> CreateSegmentsSpeed(Segments segments)
        {
            return segments.Speed.Select((item, index) => new { Type = (SegmentType)index, Speed = item })
                                 .ToDictionary(item => item.Type, item => item.Speed);
        }

        public Map GetMap() => map.Value;
    }
}
