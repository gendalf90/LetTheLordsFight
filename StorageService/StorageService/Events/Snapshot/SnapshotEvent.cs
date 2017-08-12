using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class SnapshotEvent : Event
    {
        public SnapshotEvent(string id, string storageId, IReadOnlyDictionary<string, int> items) 
            : base(id, storageId)
        {
            Items = items;
        }

        public IReadOnlyDictionary<string, int> Items { get; private set; }

        public override async Task AcceptAsync(IEventVisitor visitor)
        {
            await visitor.VisitAsync(this);
        }
    }
}
