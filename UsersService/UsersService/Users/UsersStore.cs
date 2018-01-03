using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using UsersDomain.Entities;
using UsersDomain.Exceptions;

namespace UsersService.Users
{
    class UsersStore : IUsersStore
    {
        private const int MySqlDublicateEntryErrorNumber = 1062;

        private readonly UsersContext context;

        public UsersStore(UsersContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(User user)
        {
            var userData = CreateUserDataFrom(user);
            await TrySaveUserDataAsync(userData);
        }

        private UserData CreateUserDataFrom(User user)
        {
            var data = new UserData();
            user.FillRepositoryData(data);
            return data;
        }

        private async Task TrySaveUserDataAsync(UserData data)
        {
            try
            {
                await SaveUserDataAsync(data);
            }
            catch (DbUpdateException e)
            {
                ThrowIfThisExceptionIsDublicateEntryError(e);
            }
        }

        private async Task SaveUserDataAsync(UserData data)
        {
            context.Users.Add(data);
            await context.SaveChangesAsync();
        }

        private void ThrowIfThisExceptionIsDublicateEntryError(DbUpdateException exception)
        {
            var inner = exception.InnerException as MySqlException;

            if (inner?.Number == MySqlDublicateEntryErrorNumber)
            {
                throw new UserAlreadyExistException($"login already exist");
            }
        }

        public async Task<User> GetAsync(string login)
        {
            var data = await context.Users.SingleAsync(record => record.Login == login);
            return new User(data);
        }
    }
}
