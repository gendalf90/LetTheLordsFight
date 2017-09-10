using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsersDomain.Exceptions;
using UsersDomain.Repositories;

namespace UsersDomain.Services
{
    public class UserValidationService : IUserValidationService
    {
        private readonly IUsersRepository usersRepository;

        public UserValidationService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task CurrentUserShouldBeSystemOrAdminAsync()
        {
            var currentUser = await usersRepository.GetCurrentAsync();

            if(!currentUser.IsSystemOrAdmim)
            {
                throw new UserValidationException();
            }
        }
    }
}
