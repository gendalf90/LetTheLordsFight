using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UsersService.Database;
using UserDatabaseDto = UsersService.Database.UserDto;

namespace UsersService.BasicAuthentication
{
    class GetUserByLoginAndPasswordStrategy : IGetUserByLoginAndPasswordStrategy
    {
        private readonly Context dbContext;

        public GetUserByLoginAndPasswordStrategy(Context dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<UserDto> GetAsync(string login, string password)
        {
            var dbUserData = await dbContext.Users.Include(user => user.Roles)
                                                  .Where(user => string.Equals(user.Login, login, StringComparison.OrdinalIgnoreCase))
                                                  .Where(user => string.Equals(user.Password, password, StringComparison.Ordinal))
                                                  .SingleOrDefaultAsync();

            if(dbUserData == null)
            {
                return null;
            }

            return MapFromDatabaseDto(dbUserData);
        }

        private UserDto MapFromDatabaseDto(UserDatabaseDto dto)
        {
            return new UserDto
            {
                Login = dto.Login,
                Roles = dto.Roles.Select(role => role.Value).ToArray()
            };
        }
    }
}
