using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UsersService.Controllers.Data;

namespace UsersService.Queries.GetCurrentToken
{
    public class Query : IQuery<TokenDto>
    {
        private readonly IGetCurrentUserStrategy currentUser;
        private readonly IGetTokenSigningKeyStrategy signingKey;

        private IEnumerable<Claim> claims;
        private SigningCredentials signingCredentials;
        private TokenDto token;

        public Query(IGetCurrentUserStrategy currentUser, IGetTokenSigningKeyStrategy signingKey)
        {
            this.currentUser = currentUser;
            this.signingKey = signingKey;
        }

        public Task<TokenDto> AskAsync()
        {
            CreateClaims();
            CreateCredentials();
            CreateToken();
            return Task.FromResult(token);
        }

        private void CreateClaims()
        {
            var currentUserData = currentUser.Get();
            claims = GetClaimsForUser(currentUserData);
        }

        private IEnumerable<Claim> GetClaimsForUser(UserDto dto)
        {
            yield return new Claim(ClaimTypes.Name, dto.Login);

            foreach (var role in dto.Roles)
            {
                yield return new Claim(ClaimTypes.Role, role);
            }
        }

        private void CreateCredentials()
        {
            var signingKeyString = signingKey.Get();
            var signingKeyBytes = Encoding.ASCII.GetBytes(signingKeyString);
            var securityKey = new SymmetricSecurityKey(signingKeyBytes);
            signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private void CreateToken()
        {
            var jwtToken = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            token = new TokenDto
            {
                Token = jwtTokenHandler.WriteToken(jwtToken)
            };
        }
    }
}
