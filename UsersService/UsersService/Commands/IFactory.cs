using System;
using UsersService.Common;

namespace UsersService.Commands
{
    public interface IFactory
    {
        ICommand GetCreateRegistrationRequestCommand(RegistrationData data);

        ICommand GetRegisterUserCommand(Guid requestId);
    }
}
