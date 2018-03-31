using System;
using UsersDomain.Repositories;
using UsersDomain.Repositories.Registration;
using UsersDomain.Services.Registration;
using UsersService.Commands.CreateRegistrationRequest;
using UsersService.Common;
using UsersService.Logs;
using CreateRegistrationRequestCommand = UsersService.Commands.CreateRegistrationRequest.Command;
using RegisterUserCommand = UsersService.Commands.RegisterUser.Command;

namespace UsersService.Commands
{
    class Factory : IFactory
    {
        private readonly IConfirmationLink confirmationLink;
        private readonly IUsers users;
        private readonly IRequests requests;
        private readonly IEmail email;
        private readonly ILog log;

        public Factory(IUsers users, 
                       IRequests requests, 
                       IEmail email, 
                       IConfirmationLink confirmationLink,
                       ILog log)
        {
            this.users = users;
            this.requests = requests;
            this.email = email;
            this.confirmationLink = confirmationLink;
            this.log = log;
        }

        public ICommand GetCreateRegistrationRequestCommand(RegistrationData data)
        {
            return new CreateRegistrationRequestCommand(confirmationLink, requests, email, log, data);
        }

        public ICommand GetRegisterUserCommand(Guid requestId)
        {
            return new RegisterUserCommand(requests, users, log, requestId);
        }
    }
}
