using StorageDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Events
{
    class SingleTransactionEvent : Event
    {
        public SingleTransactionEvent(string id, SingleTransactionType transactionType, string storageId, string itemName, int itemCount) 
            : base(id, storageId)
        {
            ItemName = itemName;
            ItemCount = itemCount;
            TransactionType = transactionType;
        }

        public SingleTransactionType TransactionType { get; private set; }

        public string ItemName { get; private set; }

        public int ItemCount { get; private set; }

        public override async Task AcceptAsync(IEventVisitor visitor)
        {
            await visitor.VisitAsync(this);
        }
    }
}
