using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageDomain.Entities;
using StorageService.Events;
using StorageService.Commands;

namespace StorageService.Storages
{
    class StorageStore : IStorageStore
    {
        private readonly IEventStore eventStore;
        private readonly IEventVisitorFactory visitorsFactory;

        public StorageStore(IEventStore eventStore, IEventVisitorFactory visitorsFactory)
        {
            this.eventStore = eventStore;
            this.visitorsFactory = visitorsFactory;
        }

        public async Task<Storage> RestoreStorageToThisExclusiveEventIdAsync(string eventId, string storageId)
        {
            var allEvents = await eventStore.GetByStorageIdAsync(storageId);

            if(!allEvents.Any(e => e.Id == eventId))
            {
                throw new Exception();
            }

            var eventsToRestore = allEvents.TakeWhile(e => e.Id != eventId);
            return await RestoreSingleStorageFromEvents(eventsToRestore);
        }

        public async Task<Storage> RestoreStorageToLastEventAsync(string storageId)
        {
            var events = await eventStore.GetByStorageIdAsync(storageId);
            return await RestoreSingleStorageFromEvents(events);
        }

        private async Task<Storage> RestoreSingleStorageFromEvents(IEnumerable<Event> events)
        {
            var restored = new List<Storage>(1);
            var applyVisitor = visitorsFactory.CreateApplyVisitor(restored);

            foreach (var e in events)
            {
                await e.AcceptAsync(applyVisitor);
            }

            return restored.Single();
        }
    }
}
