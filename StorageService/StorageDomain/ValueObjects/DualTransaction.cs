using StorageDomain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageDomain.ValueObjects
{
    public class DualTransaction
    {
        public DualTransaction(string fromStorageId, string toStorageId, Item item)
        {
            Decrease = new SingleTransaction(fromStorageId, SingleTransactionType.Decrease, item);
            Increase = new SingleTransaction(toStorageId, SingleTransactionType.Increase, item);
        }

        public SingleTransaction Decrease { get; private set; }

        public SingleTransaction Increase { get; private set; }
    }
}
