using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Services
{
    interface IUsersService
    {
        Task<UserDto> GetCurrentAsync();

        Task<UserDto> GetByStorageIdAsync(string storageId);
    }
}
