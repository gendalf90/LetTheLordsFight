using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorageDomain.Services
{
    public interface IDistanceService
    {
        Task<bool> IsDistanceBeetwenStoragesSufficientForTransactionAsync(string fromId, string toId);
    }
}
