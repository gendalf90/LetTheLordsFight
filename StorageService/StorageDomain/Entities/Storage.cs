using StorageDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using StorageDomain.Common;
using StorageDomain.Exceptions;

namespace StorageDomain.Entities
{
    public class Storage
    {
        private Dictionary<string, Item> items;

        private SingleTransaction currentTransaction;

        public Storage(string id)
        {
            Id = id;
            items = new Dictionary<string, Item>();
        }

        public Storage(StorageRepositoryData repositoryData)
        {
            Id = repositoryData.Id;
            items = repositoryData.Items.ToDictionary(item => item.Name, item => item);
        }

        public string Id { get; private set; }

        public StorageRepositoryData GetRepositoryData()
        {
            return new StorageRepositoryData { Id = Id, Items = items.Values.ToList() };
        }

        public bool IsTransactionPossible(SingleTransaction transaction)
        {
            currentTransaction = transaction;

            if(CurrentIdNotEqualTransactionId)
            {
                return false;
            }

            if(CurrentTransactionTypeIsDecrease && CurrentItemQuantityLessThanTransactionItemOrEmpty)
            {
                return false;
            }

            return true;
        }

        private bool CurrentIdNotEqualTransactionId
        {
            get
            {
                return currentTransaction.StorageId != Id;
            }
        }

        private bool CurrentTransactionTypeIsDecrease
        {
            get
            {
                return currentTransaction.Type == SingleTransactionType.Decrease;
            }
        }

        private bool CurrentItemQuantityLessThanTransactionItemOrEmpty
        {
            get
            {
                return !items.TryGetValue(currentTransaction.Item.Name, out Item currentItem) || currentItem.Quantity < currentTransaction.Item.Quantity;
            }
        }

        public void ApplyTransaction(SingleTransaction transaction)
        {
            if(!IsTransactionPossible(transaction))
            {
                throw new ValidationException();
            }

            if(transaction.Type == SingleTransactionType.Increase)
            {
                IncreaseItemQuantity(transaction.Item);
            }
            else
            {
                DecreaseItemQuantity(transaction.Item);
            }
        }

        private void IncreaseItemQuantity(Item item)
        {
            if (items.TryGetValue(item.Name, out Item current))
            {
                items[item.Name] = new Item(item.Name, current.Quantity + item.Quantity);
            }
            else
            {
                items[item.Name] = item;
            }
        }

        private void DecreaseItemQuantity(Item item)
        {
            var current = items[item.Name];

            if (current.Quantity > item.Quantity)
            {
                items[item.Name] = new Item(item.Name, current.Quantity - item.Quantity);
            }
            else
            {
                items.Remove(item.Name);
            }
        }
    }
}
