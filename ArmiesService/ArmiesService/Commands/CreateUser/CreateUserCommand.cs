using ArmiesDomain.Entities;
using ArmiesDomain.Repositories.Users;
using ArmiesService.Consumers.Data;
using ArmiesService.Logs;
using System;
using System.Threading.Tasks;

namespace ArmiesService.Commands.CreateUser
{
    public class CreateUserCommand : ICommand
    {
        private readonly IUsers users;
        private readonly ILog logger;
        private readonly UserConsumerDto data;

        private User user;

        public CreateUserCommand(IUsers users, ILog logger, UserConsumerDto data)
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
