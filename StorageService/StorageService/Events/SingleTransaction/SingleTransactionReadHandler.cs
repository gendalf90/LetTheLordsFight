using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;
using StorageDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class SingleTransactionReadHandler : JsonReadHandler
    {
        protected override bool TryParse(string json, out Event result)
        {
            result = null;
            var obj = JObject.Parse(json);

            if (obj.Value<string>("Type") != "SingleTransaction")
            {
                return false;
            }

            var id = obj.Value<string>("_id");
            var storageId = obj.Value<string>("StorageId");
            var itemName = obj.Value<string>("ItemName");
            var itemCount = obj.Value<int>("ItemCount");
            var transactionType = obj.Value<string>("TransactionType");

            result = new SingleTransactionEvent(id,
                                                Enum.Parse<SingleTransactionType>(transactionType),
                                                storageId,
                                                itemName,
                                                itemCount);
            return true;
        }
    }
}
