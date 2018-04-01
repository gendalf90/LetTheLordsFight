using Microsoft.Extensions.Options;
using System;
using UsersService.Options;

namespace UsersService.Commands.CreateRegistrationRequest
{
    public class ConfirmationLink : IConfirmationLink
    {
        private readonly IOptions<ConfirmationOptions> options;

        public ConfirmationLink(IOptions<ConfirmationOptions> options)
        {
            this.options = options;
        }

        public string GetForRequestId(Guid requestId)
        {
            return $"{options.Value.Link}/{requestId.ToString()}";
        }
    }
}
