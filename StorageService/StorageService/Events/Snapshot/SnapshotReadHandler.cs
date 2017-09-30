using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace StorageService.Events
{
    class SnapshotReadHandler : JsonReadHandler
    {
        protected override bool TryParse(string json, out Event result)
        {
            result = null;
            var obj = JObject.Parse(json);

            if (obj.Value<string>("Type") != "Snapshot")
            {
                return false;
            }

            var id = obj.Value<string>("_id");
            var storageId = obj.Value<string>("StorageId");
            var items = obj["Items"].ToObject<Dictionary<string, int>>();

            result = new SnapshotEvent(id, storageId, items);
            return true;
        }
    }
}
