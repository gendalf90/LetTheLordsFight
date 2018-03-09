using System;
using UsersDomain.Repositories;
using UsersDomain.Repositories.Registration;
using UsersDomain.Services.Registration;
using UsersService.Common;

namespace UsersService.Commands
{
    class Factory : IFactory
    {
        private readonly IUsers users;
        private readonly IRequests requests;
        private readonly IEmail email;

        public Factory(IUsers users, IRequests requests, IEmail email)
        {
            this.users = users;
            this.requests = requests;
            this.email = email;
        }

        public ICommand GetCreateRegistrationRequestCommand(RegistrationData data)
        {
            throw new NotImplementedException();
        }

        public ICommand GetRegisterUserCommand(Guid requestId)
        {
            throw new NotImplementedException();
        }
    }
}
