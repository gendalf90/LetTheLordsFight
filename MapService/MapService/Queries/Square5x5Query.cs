using MapDomain.Repositories;
using MapDomain.ValueObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Linq;
using MapDomain.Factories;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace MapService.Queries
{
    public class Square5x5Query : IQuery
    {
        private const int Size = 5;

        private readonly IMongoCollection<BsonDocument> objects;
        private readonly IMapFactory mapFactory;

        private int upI;
        private int leftJ;
        private int downI;
        private int rightJ;
        private Map map;

        public Square5x5Query(IMongoDatabase mapDatabase, IMapFactory factory, int upI, int leftJ)
        {
            objects = mapDatabase.GetCollection<BsonDocument>("objects");
            mapFactory = factory;
            this.upI = upI;
            this.leftJ = leftJ;
        }

        public async Task<string> GetJsonAsync()
        {
            InitializeMap();
            FillDownIAndRightJ();
            return await CreateResultAsync();
        }

        private void InitializeMap()
        {
            map = mapFactory.GetMap();
        }

        private void FillDownIAndRightJ()
        {
            var maxI = map.SegmentsHeight - 1;
            var currentI = upI + Size;
            var maxJ = map.SegmentsWidth - 1;
            var currentJ = leftJ + Size;
            downI = currentI > maxI ? maxI : currentI;
            rightJ = currentJ > maxJ ? maxJ : currentJ;
        }

        private async Task<string> CreateResultAsync()
        {
            var segments = LoadSegments();
            var objects = await LoadObjectsAsync();
            var segmentsData = segments.Select(CreateSegmentData);
            var objectsData = objects.Select(CreateObjectData);

            var result = new JObject
            {
                ["segments"] = JArray.FromObject(segmentsData),
                ["objects"] = JArray.FromObject(objectsData)
            };

            return result.ToString();
        }

        private IEnumerable<Segment> LoadSegments()
        {
            for (int i = upI; i <= downI; i++)
            {
                for (int j = leftJ; j <= rightJ; j++)
                {
                    yield return map[i, j];
                }
            }
        }

        private JObject CreateSegmentData(Segment segment)
        {
            return new JObject
            {
                ["I"] = segment.I,
                ["J"] = segment.J,
                ["LeftX"] = segment.LeftUpLocation.X,
                ["UpY"] = segment.LeftUpLocation.Y,
                ["RightX"] = segment.RightDownLocation.X,
                ["DownY"] = segment.RightDownLocation.Y,
                ["Type"] = segment.Type.ToString()
            };
        }

        private async Task<IEnumerable<BsonDocument>> LoadObjectsAsync()
        {
            var leftUpSegment = map[upI, leftJ];
            var rightDownSegment = map[downI, rightJ];

            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.And(builder.Eq("IsVisible", true),
                                     builder.Gt("LocationX", leftUpSegment.LeftUpLocation.X),
                                     builder.Lt("LocationX", rightDownSegment.RightDownLocation.X),
                                     builder.Gt("LocationY", leftUpSegment.LeftUpLocation.Y),
                                     builder.Lt("LocationY", rightDownSegment.RightDownLocation.Y));
            var projection = Builders<BsonDocument>.Projection.Include("LocationX")
                                                              .Include("LocationY");

            return await objects.Find(filter)
                                .Project(projection)
                                .ToListAsync();
        }

        private JObject CreateObjectData(BsonDocument bson)
        {
            return new JObject
            {
                ["id"] = bson["_id"].AsString,
                ["Location"] = new JObject
                {
                    {"X", bson["LocationX"].AsDouble },
                    {"Y", bson["LocationY"].AsDouble }
                }
            };
        }
    }
}
