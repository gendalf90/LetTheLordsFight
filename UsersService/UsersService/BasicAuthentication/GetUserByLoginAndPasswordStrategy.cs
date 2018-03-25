using System;
using System.Threading.Tasks;

namespace UsersService.BasicAuthentication
{
    class GetUserByLoginAndPasswordStrategy : IGetUserByLoginAndPasswordStrategy
    {
        public Task<UserDto> GetAsync(string login, string password)
        {
            throw new NotImplementedException();
        }
    }
}
