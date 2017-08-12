using MapDomain.Exceptions;
using MapDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.Services
{
    public class UserValidationService : IUserValidationService
    {
        private readonly IUsersRepository userRepository;

        public UserValidationService(IUsersRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void CurrentUserShouldBeOwnerOfThisMapObject(string mapObjectId)
        {
            var currentUser = userRepository.GetCurrent();

            if(!currentUser.IsOwnerOf(mapObjectId))
            {
                throw new NoPermissionException();
            }
        }

        public void CurrentUserShouldBeSystemOrAdmin()
        {
            var currentUser = userRepository.GetCurrent();

            if (!currentUser.IsAdminOrSystem)
            {
                throw new NoPermissionException();
            }
        }
    }
}
