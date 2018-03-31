using System;

namespace UsersService.Commands.CreateRegistrationRequest
{
    public interface IConfirmationLink
    {
        string GetForRequestId(Guid requestId);
    }
}
