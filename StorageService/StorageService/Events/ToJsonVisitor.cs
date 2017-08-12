using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class ToJsonVisitor : IEventVisitor
    {
        private readonly ICollection<string> source;

        public ToJsonVisitor(ICollection<string> source)
        {
            this.source = source;
        }

        public async Task VisitAsync(SingleTransactionEvent e)
        {
            source.Add(ToJson(e));
            await Task.CompletedTask;
        }

        public async Task VisitAsync(CreateEvent e)
        {
            source.Add(ToJson(e));
            await Task.CompletedTask;
        }

        public async Task VisitAsync(SnapshotEvent e)
        {
            source.Add(ToJson(e));
            await Task.CompletedTask;
        }

        private string ToJson<T>(T e) where T : Event
        {
            var document = e.ToBsonDocument();
            document["Type"] = nameof(T);
            return document.ToJson();
        }

        private string ToJson(SnapshotEvent e)
        {
            var itemsElements = e.Items.Select(pair => new BsonElement(pair.Key, pair.Value));
            var items = new BsonDocument(itemsElements);
            var document = new BsonDocument
            {
                ["_id"] = e.Id,
                ["Type"] = nameof(SnapshotEvent),
                [nameof(e.StorageId)] = e.StorageId,
                [nameof(e.Items)] = items
            };
            return document.ToJson();
        }
    }
}
