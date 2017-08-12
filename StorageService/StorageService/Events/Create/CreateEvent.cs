using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class CreateEvent : Event
    {
        public CreateEvent(string id, string storageId) 
            : base(id, storageId)
        {
        }

        public override async Task AcceptAsync(IEventVisitor visitor)
        {
            await visitor.VisitAsync(this);
        }
    }
}
