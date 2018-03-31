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
            var builder = new UriBuilder(options.Value.Link);
            builder.Path = requestId.ToString();
            return builder.ToString();
        }
    }
}
