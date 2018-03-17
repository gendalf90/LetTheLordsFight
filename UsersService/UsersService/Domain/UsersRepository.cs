using System;
using System.Threading.Tasks;
using UsersDomain.Repositories;

namespace UsersService.Domain
{
    public class UsersRepository : IUsers
    {
        public Task SaveAsync(UserDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
