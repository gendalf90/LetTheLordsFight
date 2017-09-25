using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorageDomain.Services
{
    public interface IUserService
    {
        bool IsCurrentUserOwnerOfThisStorageOrSystem(string storageId);

        bool IsCurrentUserSystem();
    }
}
