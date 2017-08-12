using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    abstract class Event
    {
        public Event(string id, string storageId)
        {
            Id = id;
            StorageId = storageId;
        }

        public string Id { get; private set; }

        public string StorageId { get; private set; }

        public abstract Task AcceptAsync(IEventVisitor visitor);
    }
}
