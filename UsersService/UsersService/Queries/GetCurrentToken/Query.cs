using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UsersService.Options;

namespace UsersService.Queries.GetCurrentToken
{
    public class Query : IQuery<string>
    {
        private readonly IHttpContextAccessor context;
        private readonly IOptions<JwtOptions> options;

        public Query(IOptions<JwtOptions> options, IHttpContextAccessor context)
        {
            this.options = options;
            this.context = context;
        }

        public Task<string> AskAsync()
        {
            var token = CreateToken();
            return Task.FromResult(token);
        }

        private string CreateToken()
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = new JwtSecurityToken(signingCredentials: TokenSign, claims: TokenClaims);
            return handler.WriteToken(jwt);
        }

        private IEnumerable<Claim> TokenClaims
        {
            get
            {
                return context.HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Name || claim.Type == ClaimTypes.Role);
            }
        }

        private SigningCredentials TokenSign
        {
            get
            {
                return options.Value.Sign;
            }
        }
    }
}
