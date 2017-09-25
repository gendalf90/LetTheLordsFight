using StorageDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorageDomain.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository users;

        public UserService(IUsersRepository users)
        {
            this.users = users;
        }

        public bool IsCurrentUserOwnerOfThisStorageOrSystem(string storageId)
        {
            var currentUser = users.GetCurrent();
            return currentUser.IsSystem || currentUser.IsOwnerOf(storageId);
        }

        public bool IsCurrentUserSystem()
        {
            var currentUser = users.GetCurrent();
            return currentUser.IsSystem;
        }
    }
}
