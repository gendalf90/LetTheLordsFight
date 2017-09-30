using Microsoft.Extensions.Internal;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using StorageService.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class EventStore : IEventStore
    {
        private readonly IEventVisitorFactory visitorsFactory;
        private readonly IEventReaderCreator readerCreator;
        private readonly IMongoCollection<BsonDocument> eventsCollection;
        private readonly ISystemClock time;

        public EventStore(IEventVisitorFactory visitorsFactory, 
                          IEventReaderCreator readerCreator, 
                          IMongoDatabase storageDatabase,
                          ISystemClock time)
        {
            this.time = time;
            this.visitorsFactory = visitorsFactory;
            this.readerCreator = readerCreator;
            eventsCollection = storageDatabase.GetCollection<BsonDocument>("events");
        }

        public async Task<IEnumerable<Event>> GetByStorageIdAsync(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var container = await eventsCollection.Find(filter).FirstAsync();
            var events = container["Events"].AsBsonArray;
            return FromDocuments(events);
        }

        private IEnumerable<Event> FromDocuments(IEnumerable<BsonValue> documents)
        {
            var reader = readerCreator.Create();
            var settings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
            return documents.Select(document => document.ToJson(settings))
                            .Select(reader.ReadFromJson);
        }

        public async Task AddAsync(Event e)
        {
            var document = await ToDocumentAsync(e);
            SetCurrentTime(document);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", e.StorageId);
            var update = Builders<BsonDocument>.Update.Push("Events", document);
            await eventsCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        private async Task<BsonDocument> ToDocumentAsync(Event e)
        {
            var converted = new List<string>(1);
            var toJsonConverter = visitorsFactory.CreateToJsonVisitor(converted);
            await e.AcceptAsync(toJsonConverter);
            return BsonDocument.Parse(converted.Single());
        }

        private void SetCurrentTime(BsonDocument document)
        {
            document["Time"] = time.UtcNow.DateTime;
        }
    }
}
