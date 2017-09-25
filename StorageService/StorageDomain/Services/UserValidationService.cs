using StorageDomain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageDomain.Services
{
    public class UserValidationService : IUserValidationService
    {
        private readonly IUserService userService;

        public UserValidationService(IUserService userService)
        {
            this.userService = userService;
        }

        public void CurrentUserShouldBeOwnerOfThisStorageOrSystem(string storageId)
        {
            if(!userService.IsCurrentUserOwnerOfThisStorageOrSystem(storageId))
            {
                throw new NotAuthorizedException();
            }
        }

        public void CurrentUserShouldBeSystem()
        {
            if(!userService.IsCurrentUserSystem())
            {
                throw new NotAuthorizedException();
            }
        }
    }
}
