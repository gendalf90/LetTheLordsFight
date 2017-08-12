using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Queries
{
    interface IQueryFactory
    {
        IQuery CreateStorageQuery(string storageId);
    }
}
