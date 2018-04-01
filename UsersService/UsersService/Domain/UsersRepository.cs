using System.Threading.Tasks;
using UsersDomain.Repositories;
using UsersService.Database;
using DomainUserDto = UsersDomain.Repositories.UserDto;
using DatabaseUserDto = UsersService.Database.UserDto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UsersDomain.Exceptions;

namespace UsersService.Domain
{
    class UsersRepository : IUsers
    {
        private readonly Context dbContext;

        public UsersRepository(Context dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveAsync(DomainUserDto dto)
        {
            var databaseDto = ToDatabaseUserDto(dto);
            await TrySaveAsync(databaseDto);
        }

        private DatabaseUserDto ToDatabaseUserDto(DomainUserDto dto)
        {
            var roles = dto.Roles.Select(role => new RoleDto { UserId = dto.Id, Value = role.ToString() });

            return new DatabaseUserDto
            {
                Id = dto.Id,
                Login = dto.Login,
                Password = dto.Password,
                Roles = roles.ToArray()
            };
        }

        private async Task TrySaveAsync(DatabaseUserDto dto)
        {
            try
            {
                dbContext.Users.Add(dto);
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                e.ThrowIfDublicateEntry(new UserException($"Attempt to save dublicate of user with id {dto.Id}"));
                throw;
            }
        }
    }
}
