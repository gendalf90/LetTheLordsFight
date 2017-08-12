using System;
using System.Collections.Generic;
using System.Text;

namespace StorageDomain.ValueObjects
{
    public enum SingleTransactionType : byte
    {
        Increase = 0,
        Decrease = 1
    }
}
