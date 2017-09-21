using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersDomain.Entities;
using UsersDomain.ValueTypes;
using UsersService.Common;
using UsersService.Users;

namespace UsersService.Commands
{
    public class CreateUserCommand : ICommand
    {
        private readonly IUsersStore store;
        private readonly CreateUserData data;

        private User newUser;

        public CreateUserCommand(IUsersStore store, CreateUserData data)
        {
            this.store = store;
            this.data = data;
        }

        public async Task ExecuteAsync()
        {
            CreateNewUser();
            await AddNewUserToStoreAsync();
        }

        private void CreateNewUser()
        {
            var login = new Login(data.Login);
            var password = new Password(data.Password);
            newUser = new User(login, password);
        }

        private async Task AddNewUserToStoreAsync()
        {
            await store.SaveAsync(newUser);
        }
    }
}
