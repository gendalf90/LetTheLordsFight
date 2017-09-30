using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class CreateReadHandler : JsonReadHandler
    {
        protected override bool TryParse(string json, out Event result)
        {
            result = null;
            var obj = JObject.Parse(json);
            
            if(obj.Value<string>("Type") != "Create")
            {
                return false;
            }

            var id = obj.Value<string>("_id");
            var storageId = obj.Value<string>("StorageId");
            result = new CreateEvent(id, storageId);
            return true;
        }
    }
}
