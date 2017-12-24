using MapDomain.Factories;
using MapDomain.Services;
using MapDomain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MapService.Queries
{
    public class ObjectQuery : IQuery
    {
        private readonly IMongoCollection<BsonDocument> objects;
        private readonly IUserValidationService validation;
        private readonly IMapFactory mapFactory;
        private readonly string id;

        private BsonDocument objectBson;
        private Segment objectSegment;
        private JObject objectJson;

        public ObjectQuery(IMongoDatabase mapDatabase, IMapFactory mapFactory, IUserValidationService validation, string id)
        {
            objects = mapDatabase.GetCollection<BsonDocument>("objects");

            this.mapFactory = mapFactory;
            this.validation = validation;
            this.id = id;
        }

        public async Task<JObject> GetJsonAsync()
        {
            Validate();
            await LoadObjectDataAsync();
            ConvertObjectDataToJson();
            LoadSegmentData();
            AddSegmentDataToObjectData();
            return GetResult();
        }

        private void Validate()
        {
            validation.CurrentCanViewThisMapObject(id);
        }

        private async Task LoadObjectDataAsync()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var projection = Builders<BsonDocument>.Projection.Include("LocationX")
                                                              .Include("LocationY")
                                                              .Include("IsVisible");
            objectBson = await objects.Find(filter)
                                      .Project(projection)
                                      .FirstAsync();
        }
        
        private void ConvertObjectDataToJson()
        {
            objectJson = new JObject
            {
                ["Id"] = objectBson["_id"].AsString,
                ["Location"] = new JObject
                {
                    {"X", objectBson["LocationX"].AsDouble },
                    {"Y", objectBson["LocationY"].AsDouble }
                },
                ["Visible"] = objectBson["IsVisible"].AsBoolean
            };
        }

        private void LoadSegmentData()
        {
            var map = mapFactory.GetMap();
            var x = objectJson.Value<float>("Location.X");
            var y = objectJson.Value<float>("Location.Y");
            objectSegment = map[x, y];
        }

        private void AddSegmentDataToObjectData()
        {
            objectJson["Segment"] = new JObject
            {
                {"I", objectSegment.I },
                {"J", objectSegment.J }
            };
        }

        private JObject GetResult()
        {
            return objectJson;
        }
    }
}
