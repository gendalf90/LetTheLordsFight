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

        public void CurrentCanCreateMapObject()
        {
            CurrentUserShouldBeSystem();
        }

        public void CurrentCanChangeDestinationForThisMapObject(string mapObjectId)
        {
            CurrentUserShouldBeOwnerOfThisMapObjectOrSystem(mapObjectId);
        }

        public void CurrentCanViewThisMapObject(string mapObjectId)
        {
            CurrentUserShouldBeOwnerOfThisMapObjectOrSystem(mapObjectId);
        }

        public void CurrentCanUpdateMap()
        {
            CurrentUserShouldBeSystem();
        }

        private void CurrentUserShouldBeOwnerOfThisMapObjectOrSystem(string mapObjectId)
        {
            var currentUser = userRepository.GetCurrent();

            if (!currentUser.IsOwnerOf(mapObjectId) && !currentUser.IsSystem)
            {
                throw new NoPermissionException();
            }
        }

        private void CurrentUserShouldBeSystem()
        {
            var currentUser = userRepository.GetCurrent();

            if (!currentUser.IsSystem)
            {
                throw new NoPermissionException();
            }
        }
    }
}
