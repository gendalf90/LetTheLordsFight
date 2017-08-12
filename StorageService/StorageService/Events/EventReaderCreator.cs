using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class EventReaderCreator : IEventReaderCreator
    {
        public IEventReader Create()
        {
            var createEventHandler = new CreateReadHandler();
            var singleTransactionEventHandler = new SingleTransactionReadHandler();
            var aggregateEventHandler = new SnapshotReadHandler();

            createEventHandler.SetSuccessor(singleTransactionEventHandler);
            singleTransactionEventHandler.SetSuccessor(aggregateEventHandler);

            return createEventHandler;
        }
    }
}
