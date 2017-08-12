using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace StorageService.Events
{
    class SnapshotReadHandler : BsonReadHandler<SnapshotEvent>
    {
        protected override SnapshotEvent ToEventFromBsonDocument(BsonDocument document)
        {
            var id = document["_id"].AsString;
            var storageId = document["StorageId"].AsString;
            var items = document["Items"].AsBsonDocument;

            return new SnapshotEvent(id, storageId, items.ToDictionary(element => element.Name, element => element.Value.AsInt32));
        }
    }
}
