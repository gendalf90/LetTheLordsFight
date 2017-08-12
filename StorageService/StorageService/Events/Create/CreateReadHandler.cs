using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class CreateReadHandler : BsonReadHandler<CreateEvent>
    {
        protected override CreateEvent ToEventFromBsonDocument(BsonDocument document)
        {
            return new CreateEvent(document["_id"].AsString, document["StorageId"].AsString);
        }
    }
}
