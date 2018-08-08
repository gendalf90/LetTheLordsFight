using System;
using System.Threading.Tasks;
using UsersDomain.Entities;
using UsersDomain.Entities.Registration;
using UsersDomain.Repositories;
using UsersDomain.Repositories.Registration;
using UsersDomain.Services.Registration;
using UsersService.Logs;

namespace UsersService.Commands.RegisterUser
{
    public class Command : ICommand
    {
        private readonly IRequests requestRepository;
        private readonly IUsers userRepository;
        private readonly INotification notificationService;
        private readonly ILog log;

        private Guid requestId;
        private Request registrationRequest;
        private User user;

        public Command(IRequests requestRepository, 
                       IUsers userRepository, 
                       INotification notificationService, 
                       ILog log, 
                       Guid requestId)
        {
            this.requestRepository = requestRepository;
            this.userRepository = userRepository;
            this.log = log;
            this.requestId = requestId;
            this.notificationService = notificationService;
        }

        public async Task ExecuteAsync()
        {
            await LoadRequestAsync();
            CreateUser();
            await SaveUserAsync();
            await NotifyThatCreatedAsync();
            LogThatUserIsSaved();
        }

        private async Task LoadRequestAsync()
        {
            registrationRequest = await Request.LoadAsync(requestId, requestRepository);
        }

        private void CreateUser()
        {
            user = registrationRequest.CreateSimpleUser();
        }

        private async Task SaveUserAsync()
        {
            await user.SaveAsync(userRepository);
        }

        private async Task NotifyThatCreatedAsync()
        {
            await user.NotifyThatRegisteredAsync(notificationService);
        }

        private void LogThatUserIsSaved()
        {
            log.Information($"User with id {user.Id} has created from request with id {registrationRequest.Id} and saved");
        }
    }
}
