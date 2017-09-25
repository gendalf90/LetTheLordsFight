using System;
using System.Collections.Generic;
using System.Text;

namespace StorageDomain.Services
{
    public interface IUserValidationService
    {
        void CurrentUserShouldBeOwnerOfThisStorageOrSystem(string storageId);

        void CurrentUserShouldBeSystem();
    }
}
