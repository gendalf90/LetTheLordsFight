using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsersService.BasicAuthentication
{
    public interface IGetUserByLoginAndPasswordStrategy
    {
        Task<UserDto> GetAsync(string login, string password);
    }
}
