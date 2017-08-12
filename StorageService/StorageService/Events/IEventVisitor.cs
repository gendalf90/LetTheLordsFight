using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    interface IEventVisitor
    {
        Task VisitAsync(SingleTransactionEvent e);

        Task VisitAsync(CreateEvent e);

        Task VisitAsync(SnapshotEvent e);
    }
}
