using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    interface IEventStore
    {
        Task AddAsync(Event e);

        Task<IEnumerable<Event>> GetByStorageIdAsync(string id);
    }
}
