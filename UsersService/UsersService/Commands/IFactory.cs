using System;
using UsersService.Controllers.Data;

namespace UsersService.Commands
{
    public interface IFactory
    {
        ICommand GetCreateRegistrationRequestCommand(RegistrationData data);

        ICommand GetRegisterUserCommand(Guid requestId);
    }
}
