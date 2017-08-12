using StorageDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorageDomain.Services
{
    public class UserValidationService : IUserValidationService
    {
        private readonly IUsersRepository userRepository;

        public UserValidationService(IUsersRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task CurrentUserShouldBeSystemOrAdminAsync()
        {
            var currentUser = await userRepository.GetCurrentAsync();

            if(!currentUser.IsAdminOrSystem)
            {
                throw new Exception();
            }
        }

        public async Task CurrentUserShouldBeOwnerOfThisStorageAsync(string storageId)
        {
            var currentUser = await userRepository.GetCurrentAsync();

            if (!currentUser.IsOwnerOf(storageId))
            {
                throw new Exception();
            }
        }
    }
}
