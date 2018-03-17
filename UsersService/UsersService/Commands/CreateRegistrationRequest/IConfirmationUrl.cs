using System;

namespace UsersService.Commands.CreateRegistrationRequest
{
    public interface IConfirmationUrl
    {
        string GetForRequestId(Guid requestId);
    }
}
