using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using StorageDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class SingleTransactionReadHandler : BsonReadHandler<SingleTransactionEvent>
    {
        [BsonIgnoreExtraElements]
        class EventDto
        {
            public string Id { get; set; }

            public SingleTransactionType TransactionType { get; set; }

            public string StorageId { get; set; }

            public string ItemName { get; set; }

            public int ItemCount { get; set; }
        }

        protected override SingleTransactionEvent ToEventFromBsonDocument(BsonDocument document)
        {
            var dto = BsonSerializer.Deserialize<EventDto>(document);
            return new SingleTransactionEvent(dto.Id, dto.TransactionType, dto.StorageId, dto.ItemName, dto.ItemCount);
        }
    }
}
