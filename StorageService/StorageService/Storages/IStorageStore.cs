using StorageDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Storages
{
    interface IStorageStore
    {
        Task<StorageEntity> RestoreStorageToThisExclusiveEventIdAsync(string eventId, string storageId);

        Task<StorageEntity> RestoreStorageToLastEventAsync(string storageId);
    }
}
