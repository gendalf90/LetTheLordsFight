using MapDomain.ValueObjects;
using Newtonsoft.Json.Linq;
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
        private readonly IMongoDatabase database;
        private readonly IMapFactory factory;

        private int i;
        private int j;
        private Map map;
        private Square5 square;

        public Square5x5Query(IMongoDatabase database, IMapFactory factory, int i, int j)
        {
            
            this.database = database;
            this.factory = factory;
            this.i = i;
            this.j = j;
        }

        public async Task<JObject> GetJsonAsync()
        {
            CreateMap();
            CreateSquare();
            return await CreateResultAsync();
        }

        private void CreateMap()
        {
            map = factory.GetMap();
        }

        private void CreateSquare()
        {
            square = new Square5(map, i, j);
        }

        private async Task<JObject> CreateResultAsync()
        {
            var segments = LoadSegments();
            var objects = await LoadObjectsAsync();
            var segmentsData = segments.Select(CreateSegmentData);
            var objectsData = objects.Select(CreateObjectData);

            return new JObject
            {
                ["segments"] = JArray.FromObject(segmentsData),
                ["objects"] = JArray.FromObject(objectsData)
            };
        }

        private IEnumerable<Segment> LoadSegments()
        {
            for (int i = square.UpI; i <= square.DownI; i++)
            {
                for (int j = square.LeftJ; j <= square.RightJ; j++)
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
            var objects = database.GetCollection<BsonDocument>("objects");
            var leftUpSegment = map[square.UpI, square.LeftJ];
            var rightDownSegment = map[square.DownI, square.RightJ];
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
