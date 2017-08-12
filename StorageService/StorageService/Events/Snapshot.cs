using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using StorageDomain.Entities;
using StorageService.Extensions;
using StorageService.Options;
using StorageService.Time;
using System;
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
        private readonly ITime time;
        private readonly TimeSpan makeSnapshotForOlderThan;
        private readonly int startSnapshotMakingLimit;

        public Snapshot(IEventVisitorFactory visitorsFactory, 
                        IEventReaderCreator readerCreator, 
                        IMongoDatabase database,
                        ITime time,
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
            var storagesIds = await GetAllStoragesIdsAsync();
            var snapshotMakingTasks = storagesIds.Select(CreateForStorageIdAsync);
            await Task.WhenAll(snapshotMakingTasks);
        }

        private async Task<IEnumerable<string>> GetAllStoragesIdsAsync()
        {
            var ids = await eventsCollection.GetAll()
                                            .Project(document => document["_id"])
                                            .ToListAsync();
            return ids.Select(id => id.AsString);
        }

        private async Task CreateForStorageIdAsync(string storageId)
        {
            var container = await LoadContainerAsync(storageId);

            if(container == null)
            {
                return;
            }

            var oldEventsDocuments = GetOldEventDocuments(container);

            if(oldEventsDocuments.Length < startSnapshotMakingLimit)
            {
                return;
            }

            var events = GetEvents(oldEventsDocuments);
            var storage = await RestoreStorageAsync(events);
            var snapshotEvent = CreateSnapshotEvent(storage);
            var snapshotEventDocument = await ToDocumentAsync(snapshotEvent);
            await InsertEventToPositionByStorageIdAsync(storageId, snapshotEventDocument, oldEventsDocuments.Length);
            await DeleteEventsFromStorageIdAsync(storageId, oldEventsDocuments);
        }

        private async Task<BsonDocument> LoadContainerAsync(string storageId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", storageId);
            return await eventsCollection.Find(filter).FirstOrDefaultAsync();
        }

        private BsonValue[] GetOldEventDocuments(BsonDocument container)
        {
            var events = container["Events"].AsBsonArray;
            return events.TakeWhile(IsEventOld).ToArray();
        }

        private bool IsEventOld(BsonValue e)
        {
            return time.GetCurrentUtc() - e["Time"].ToUniversalTime() > makeSnapshotForOlderThan;
        }

        private IEnumerable<Event> GetEvents(IEnumerable<BsonValue> documents)
        {
            var reader = readerCreator.Create();
            return documents.Select(document => document.ToJson())
                            .Select(reader.ReadFromJson);
        }

        private async Task<StorageEntity> RestoreStorageAsync(IEnumerable<Event> events)
        {
            var storages = new List<StorageEntity>(1);
            var applyVisitor = visitorsFactory.CreateApplyVisitor(storages);

            foreach (var e in events)
            {
                await e.AcceptAsync(applyVisitor);
            }

            return storages.Single();
        }

        private Event CreateSnapshotEvent(StorageEntity storage)
        {
            var id = Guid.NewGuid().ToBase64String();
            var storageData = storage.GetRepositoryData();
            var items = storageData.Items.ToDictionary(item => item.Name, item => item.Quantity);
            return new SnapshotEvent(id, storageData.Id, items);
        }

        private async Task<BsonValue> ToDocumentAsync(Event e)
        {
            var jsonList = new List<string>(1);
            var toJsonVisitor = visitorsFactory.CreateToJsonVisitor(jsonList);
            await e.AcceptAsync(toJsonVisitor);
            return BsonDocument.Parse(jsonList.Single());
        }

        private async Task InsertEventToPositionByStorageIdAsync(string id, BsonValue e, int pos)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var insert = Builders<BsonDocument>.Update.PushEach("Events", new[] { e }, null, pos);
            await eventsCollection.UpdateOneAsync(filter, insert);
        }

        private async Task DeleteEventsFromStorageIdAsync(string id, BsonValue[] events)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var delete = Builders<BsonDocument>.Update.PullAll("Events", events);
            await eventsCollection.UpdateOneAsync(filter, delete);
        }
    }
}
