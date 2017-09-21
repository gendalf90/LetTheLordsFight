using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersDomain.Entities;

namespace UsersService.Users
{
    class UsersStore : IUsersStore
    {
        private readonly UsersContext context;

        public UsersStore(UsersContext context)
        {
            this.context = context;
        }

        public async Task SaveAsync(User user)
        {
            var data = await GetOrCreateByLoginAsync(user.Login);
            user.FillRepositoryData(data);
            await context.SaveChangesAsync();
        }

        private async Task<UserData> GetOrCreateByLoginAsync(string login)
        {
            var current = await context.Users.SingleOrDefaultAsync(record => record.Login == login) ?? new UserData();

            if(current.Id == 0)
            {
                context.Users.Add(current);
            }

            return current;
        }

        public async Task<User> GetAsync(string login)
        {
            var data = await context.Users.SingleAsync(record => record.Login == login);
            return new User(data);
        }
    }
}
