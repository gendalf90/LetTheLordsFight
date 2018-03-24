using System;
using System.Threading.Tasks;
using UsersDomain.Entities;
using UsersDomain.Entities.Registration;
using UsersDomain.Repositories;
using UsersDomain.Repositories.Registration;
using UsersService.Logs;

namespace UsersService.Commands.RegisterUser
{
    public class Command : ICommand
    {
        private readonly IRequests requestRepository;
        private readonly IUsers userRepository;
        private readonly ILog log;

        private Guid requestId;
        private Request registrationRequest;
        private User user;

        public Command(IRequests requestRepository, IUsers userRepository, ILog log, Guid requestId)
        {
            this.requestRepository = requestRepository;
            this.userRepository = userRepository;
            this.log = log;
            this.requestId = requestId;
        }

        public async Task ExecuteAsync()
        {
            LogStart();
            await LoadRequestAsync();
            CreateUser();
            LogCreateUser();
            await SaveUserAsync();
            LogSaveUser();
        }

        private void LogStart()
        {
            log.Information($"Start user registration by request id {requestId}");
        }

        private async Task LoadRequestAsync()
        {
            registrationRequest = await Request.LoadAsync(requestId, requestRepository);
        }

        private void CreateUser()
        {
            user = registrationRequest.CreateSimpleUser();
        }

        private void LogCreateUser()
        {
            log.Information($"User with id {user.Id} has created from request with id {registrationRequest.Id}");
        }

        private async Task SaveUserAsync()
        {
            await user.SaveAsync(userRepository);
        }

        private void LogSaveUser()
        {
            log.Information($"User with id {user.Id} has saved");
        }
    }
}
