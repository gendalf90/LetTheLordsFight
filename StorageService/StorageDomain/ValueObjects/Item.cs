using System;
using System.Collections.Generic;
using System.Text;

namespace StorageDomain.ValueObjects
{
    public class Item
    {
        public Item(string name, int quantity)
        {
            if(string.IsNullOrWhiteSpace(name) || quantity <= 0)
            {
                throw new ArgumentException("Item arguments is incorrect");
            }

            Name = name;
            Quantity = quantity;
        }

        public string Name { get; private set; }

        public int Quantity { get; private set; }
    }
}
