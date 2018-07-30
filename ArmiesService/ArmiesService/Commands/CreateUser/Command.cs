using ArmiesDomain.Entities;
using ArmiesDomain.Repositories.Users;
using ArmiesService.Logs;
using System;
using System.Threading.Tasks;
using UserDtoOfConsumer = ArmiesService.Consumers.Data.UserDto;

namespace ArmiesService.Commands.CreateUser
{
    public class Command : ICommand
    {
        private readonly IUsers users;
        private readonly ILog logger;
        private readonly UserDtoOfConsumer data;

        private User user;

        public Command(IUsers users, ILog logger, UserDtoOfConsumer data)
        {
            this.users = users;
            this.logger = logger;
            this.data = data;
        }

        public async Task ExecuteAsync()
        {
            ValidateData();
            CreateUser();
            await SaveUserAsync();
            LogThatSaved();
        }

        private void ValidateData()
        {
            if(string.IsNullOrEmpty(data?.Login))
            {
                throw new ArgumentException(nameof(data.Login));
            }
        }

        private void CreateUser()
        {
            user = User.CreateWithLogin(data.Login);
        }

        private async Task SaveUserAsync()
        {
            await user.SaveAsync(users);
        }

        private void LogThatSaved()
        {
            logger.Information($"New user with login {user.Login} is saved");
        }
    }
}
