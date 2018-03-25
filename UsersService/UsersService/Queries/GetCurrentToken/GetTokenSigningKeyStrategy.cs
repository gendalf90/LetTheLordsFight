using Microsoft.Extensions.Options;
using UsersService.Options;

namespace UsersService.Queries.GetCurrentToken
{
    class GetTokenSigningKeyStrategy : IGetTokenSigningKeyStrategy
    {
        private readonly IOptions<JwtOptions> options;

        public GetTokenSigningKeyStrategy(IOptions<JwtOptions> options)
        {
            this.options = options;
        }

        public string Get()
        {
            return options.Value.SigningKey;
        }
    }
}
