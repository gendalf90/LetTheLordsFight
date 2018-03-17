using System;
using System.Threading.Tasks;
using UsersDomain.Entities;
using UsersDomain.Entities.Registration;
using UsersDomain.Repositories;
using UsersDomain.Repositories.Registration;

namespace UsersService.Commands.RegisterUser
{
    public class Command : ICommand
    {
        private readonly IRequests requestRepository;
        private readonly IUsers userRepository;

        private Guid requestId;
        private Request registrationRequest;
        private User user;

        public Command(IRequests requestRepository, IUsers userRepository, Guid requestId)
        {
            this.requestRepository = requestRepository;
            this.userRepository = userRepository;
            this.requestId = requestId;
        }

        public async Task ExecuteAsync()
        {
            await LoadRequestAsync();
            CreateUser();
            await SaveUserAsync();
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
    }
}
