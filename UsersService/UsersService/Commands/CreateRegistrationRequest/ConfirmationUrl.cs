using System;

namespace UsersService.Commands.CreateRegistrationRequest
{
    public class ConfirmationUrl : IConfirmationUrl
    {
        public string GetForRequestId(Guid requestId)
        {
            throw new NotImplementedException();
        }
    }
}
