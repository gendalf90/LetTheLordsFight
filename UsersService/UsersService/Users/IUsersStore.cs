using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersDomain.Entities;

namespace UsersService.Users
{
    public interface IUsersStore
    {
        Task SaveAsync(User user);

        Task<User> GetAsync(string login);
    }
}
