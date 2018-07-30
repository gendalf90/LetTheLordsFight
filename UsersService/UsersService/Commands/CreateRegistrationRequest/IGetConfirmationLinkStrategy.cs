using System;

namespace UsersService.Commands.CreateRegistrationRequest
{
    public interface IGetConfirmationLinkStrategy
    {
        string GetForRequestId(Guid requestId);
    }
}
