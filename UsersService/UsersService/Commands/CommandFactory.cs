using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersService.Common;
using UsersService.Users;

namespace UsersService.Commands
{
    class CommandFactory : ICommandFactory
    {
        private readonly IUsersStore users;

        public CommandFactory(IUsersStore users)
        {
            this.users = users;
        }

        public ICommand GetCreateUserCommand(CreateUserData data)
        {
            return new CreateUserCommand(users, data);
        }
    }
}
