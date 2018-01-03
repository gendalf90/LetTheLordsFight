using System.Threading.Tasks;
using UsersDomain.Entities;
using UsersDomain.Exceptions;
using UsersDomain.ValueTypes;
using UsersService.Common;
using UsersService.Users;

namespace UsersService.Commands
{
    public class CreateUserCommand : ICommand
    {
        private readonly IUsersStore store;
        private readonly CreateUserData data;

        private Login login;
        private Password password;
        private User user;

        public CreateUserCommand(IUsersStore store, CreateUserData data)
        {
            this.store = store;
            this.data = data;
        }

        public async Task ExecuteAsync()
        {
            CreateCredentials();
            CreateUserTypeIfNeeded();
            CreateSystemTypeIfNeeded();
            ErrorIfUserNotCreated();
            await AddNewUserToStoreAsync();
        }

        private void CreateCredentials()
        {
            login = new Login(data.Login);
            password = new Password(data.Password);
        }

        private void CreateUserTypeIfNeeded()
        {
            if(data.Type == UserType.User)
            {
                user = User.CreateUser(login, password);
            }
        }

        private void CreateSystemTypeIfNeeded()
        {
            if(data.Type == UserType.System)
            {
                user = User.CreateSystem(login, password);
            }
        }

        private void ErrorIfUserNotCreated()
        {
            if (user == null)
            {
                throw new UserTypeException("type must be user or system");
            }
        }

        private async Task AddNewUserToStoreAsync()
        {
            await store.AddAsync(user);
        }
    }
}
