using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorageDomain.Services
{
    public interface IUserValidationService
    {
        Task CurrentUserShouldBeSystemOrAdminAsync();

        Task CurrentUserShouldBeOwnerOfThisStorageAsync(string storageId);
    }
}
