using StorageDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Storages
{
    public interface IStorageStore
    {
        Task<Storage> RestoreStorageToThisExclusiveEventIdAsync(string eventId, string storageId);

        Task<Storage> RestoreStorageToLastEventAsync(string storageId);
    }
}
