using MapDomain.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Queries
{
    public class ObjectQuery : IQuery
    {
        private readonly IMongoCollection<BsonDocument> objects;
        private readonly IUserValidationService validation;
        private readonly string id;

        private BsonDocument objectBson;
        private JObject objectJson;

        public ObjectQuery(IMongoDatabase mapDatabase, IUserValidationService validation, string id)
        {
            objects = mapDatabase.GetCollection<BsonDocument>("objects");

            this.validation = validation;
            this.id = id;
        }

        public async Task<string> GetJsonAsync()
        {
            Validate();
            await LoadDataAsync();
            ConvertDataToJson();
            return GetResult();
        }

        private void Validate()
        {
            validation.CurrentCanViewThisMapObject(id);
        }

        private async Task LoadDataAsync()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var projection = Builders<BsonDocument>.Projection.Include("LocationX")
                                                              .Include("LocationY")
                                                              .Include("IsVisible");
            objectBson = await objects.Find(filter)
                                      .Project(projection)
                                      .FirstAsync();
        }

        private void ConvertDataToJson()
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

        private string GetResult()
        {
            return objectJson.ToString();
        }
    }
}
