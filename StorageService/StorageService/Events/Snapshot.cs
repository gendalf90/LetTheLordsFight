using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using StorageDomain.Entities;
using StorageService.Extensions;
using StorageService.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class Snapshot : ISnapshot
    {
        private readonly IEventVisitorFactory visitorsFactory;
        private readonly IEventReaderCreator readerCreator;
        private readonly IMongoCollection<BsonDocument> eventsCollection;
        private readonly ISystemClock time;
        private readonly TimeSpan makeSnapshotForOlderThan;
        private readonly int startSnapshotMakingLimit;
        private IList<WriteModel<BsonDocument>> writeModels;

        public Snapshot(IEventVisitorFactory visitorsFactory, 
                        IEventReaderCreator readerCreator, 
                        IMongoDatabase database,
                        ISystemClock time,
                        IOptions<EventsOptions> options)
        {
            this.visitorsFactory = visitorsFactory;
            this.readerCreator = readerCreator;
            this.time = time;
            eventsCollection = database.GetCollection<BsonDocument>("events");
            makeSnapshotForOlderThan = options.Value.MakeSnapshotForOlderThan;
            startSnapshotMakingLimit = options.Value.StartSnapshotMakingLimit;
        }

        public async Task CreateToAllAsync()
        {
            InitializeWriteModelsList();
            await UpdateStoragesIdsAsync();
            await WriteStoragesChangesAsync();
        }

        private void InitializeWriteModelsList()
        {
            writeModels = new List<WriteModel<BsonDocument>>();
        }

        private async Task UpdateStoragesIdsAsync()
        {
            await eventsCollection.GetAll().ForEachAsync(UpdateStorageAsync);
        }

        private async Task UpdateStorageAsync(BsonDocument container)
        {
            var storageId = container["_id"].AsString;
            var oldEventsDocuments = GetOldEventDocuments(container);

            if (oldEventsDocuments.Length < startSnapshotMakingLimit)
            {
                return;
            }

            var events = GetEvents(oldEventsDocuments);
            var storage = await RestoreStorageAsync(events);
            var snapshotEvent = CreateSnapshotEvent(storage);
            var snapshotEventDocument = await ToDocumentAsync(snapshotEvent);
            InsertEventToPositionByStorageId(storageId, snapshotEventDocument, oldEventsDocuments.Length);
            DeleteEventsFromStorageId(storageId, oldEventsDocuments);
        }

        private BsonDocument[] GetOldEventDocuments(BsonDocument container)
        {
            return container["Events"].AsBsonArray.Cast<BsonDocument>()
                                                  .TakeWhile(IsEventOld)
                                                  .ToArray();
        }

        private bool IsEventOld(BsonDocument e)
        {
            if(e.TryGetValue("Time", out BsonValue eventTime))
            {
                return time.UtcNow - eventTime.ToUniversalTime() > makeSnapshotForOlderThan;
            }

            return false;
        }

        private IEnumerable<Event> GetEvents(IEnumerable<BsonDocument> documents)
        {
            var reader = readerCreator.Create();
            var settings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
            return documents.Select(document => document.ToJson(settings))
                            .Select(reader.ReadFromJson);
        }

        private async Task<Storage> RestoreStorageAsync(IEnumerable<Event> events)
        {
            var storages = new List<Storage>(1);
            var applyVisitor = visitorsFactory.CreateApplyVisitor(storages);

            foreach (var e in events)
            {
                await e.AcceptAsync(applyVisitor);
            }

            return storages.Single();
        }

        private Event CreateSnapshotEvent(Storage storage)
        {
            var id = Guid.NewGuid().ToBase64String();
            var storageData = storage.GetRepositoryData();
            var items = storageData.Items.ToDictionary(item => item.Name, item => item.Quantity);
            return new SnapshotEvent(id, storageData.Id, items);
        }

        private async Task<BsonDocument> ToDocumentAsync(Event e)
        {
            var jsonList = new List<string>(1);
            var toJsonVisitor = visitorsFactory.CreateToJsonVisitor(jsonList);
            await e.AcceptAsync(toJsonVisitor);
            return BsonDocument.Parse(jsonList.Single());
        }

        private void InsertEventToPositionByStorageId(string id, BsonDocument e, int pos)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var insert = Builders<BsonDocument>.Update.PushEach("Events", new[] { e }, null, pos);
            var model = new UpdateOneModel<BsonDocument>(filter, insert);
            writeModels.Add(model);
        }

        private void DeleteEventsFromStorageId(string id, BsonDocument[] events)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var delete = Builders<BsonDocument>.Update.PullAll("Events", events);
            var model = new UpdateOneModel<BsonDocument>(filter, delete);
            writeModels.Add(model);
        }

        private async Task WriteStoragesChangesAsync()
        {
            await eventsCollection.BulkWriteAsync(writeModels);
        }
    }
}
