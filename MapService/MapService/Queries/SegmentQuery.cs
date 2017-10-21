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
    public class SegmentQuery : IQuery
    {
        private readonly IMongoCollection<BsonDocument> objects;

        private Func<Segment> getStrategy;
        private Segment segment;
        private JObject segmentData;
        private JArray objectsData;
        private IEnumerable<BsonValue> objectsBson;

        private SegmentQuery(IMongoDatabase mapDatabase)
        {
            objects = mapDatabase.GetCollection<BsonDocument>("objects");
        }

        public SegmentQuery(IMongoDatabase mapDatabase, IMapFactory factory, int i, int j)
            : this(mapDatabase)
        {
            getStrategy = () =>
            {
                var map = factory.GetMap();
                return map[i, j];
            };
        }

        public SegmentQuery(IMongoDatabase mapDatabase, IMapFactory factory, float x, float y)
            : this(mapDatabase)
        {
            getStrategy = () =>
            {
                var map = factory.GetMap();
                return map[x, y];
            };
        }

        public async Task<string> GetJsonAsync()
        {
            LoadSegment();
            CreateSegmentData();
            await LoadObjectsDataAsync();
            CreateObjectsData();
            return GetResult();
        }

        private void LoadSegment()
        {
            segment = getStrategy();
        }

        private void CreateSegmentData()
        {
            segmentData = new JObject
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

        private async Task LoadObjectsDataAsync()
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.And(builder.Eq("IsVisible", true),
                                     builder.Gt("LocationX", segment.LeftUpLocation.X),
                                     builder.Lt("LocationX", segment.RightDownLocation.X),
                                     builder.Gt("LocationY", segment.LeftUpLocation.Y),
                                     builder.Lt("LocationY", segment.RightDownLocation.Y));
            var projection = Builders<BsonDocument>.Projection.Include("LocationX")
                                                              .Include("LocationY");

            objectsBson = await objects.Find(filter)
                                       .Project(projection)
                                       .ToListAsync();
        }

        private void CreateObjectsData()
        {
            var convertedFromBsonObjects = objectsBson.Select(bson => new
            {
                Id = bson["_id"].AsString,
                Location = new
                {
                    X = bson["LocationX"].AsDouble,
                    Y = bson["LocationY"].AsDouble
                }
            });

            objectsData = JArray.FromObject(convertedFromBsonObjects);
        }

        private string GetResult()
        {
            var result = new JObject
            {
                { "segment", segmentData },
                { "objects", objectsData }
            };
            return result.ToString();
        }
    }
}
