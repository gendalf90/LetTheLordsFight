using StorageDomain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageDomain.ValueObjects
{
    public class SingleTransaction
    {
        public SingleTransaction(string storageId, SingleTransactionType type, Item item)
        {
            if(string.IsNullOrEmpty(storageId) || item == null)
            {
                throw new ArgumentException();
            }

            Type = type;
            StorageId = storageId;
            Item = item;
        }

        public SingleTransactionType Type { get; private set; }

        public string StorageId { get; private set; }

        public Item Item { get; private set; }
    }
}
