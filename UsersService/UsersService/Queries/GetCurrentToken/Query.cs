using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Queries.GetCurrentToken
{
    public class Query : IQuery<string>
    {
        private readonly IGetCurrentUserStrategy currentUser;
        private readonly IGetTokenSigningKeyStrategy signingKey;

        private UserDto currentUserData;
        private SigningCredentials signingCredentials;
        private IEnumerable<Claim> claims;
        private string resultToken;

        public Query(IGetCurrentUserStrategy currentUser, IGetTokenSigningKeyStrategy signingKey)
        {
            this.currentUser = currentUser;
            this.signingKey = signingKey;
        }

        public Task<string> AskAsync()
        {
            LoadCurrentUser();
            CreateSigningCredentials();
            CreateClaims();
            CreateToken();
            return Task.FromResult(resultToken);
        }

        private void LoadCurrentUser()
        {
            currentUserData = currentUser.Get();
        }

        private void CreateSigningCredentials()
        {
            var key = signingKey.Get();
            signingCredentials = CreateSigningCredentialsByKey(key);
        }

        private void CreateClaims()
        {
            claims = CreateClaimsForUser(currentUserData);
        }

        private void CreateToken()
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            resultToken = handler.WriteToken(jwt);
        }

        private SigningCredentials CreateSigningCredentialsByKey(string signingKey)
        {
            var bytes = Encoding.ASCII.GetBytes(signingKey);
            var key = new SymmetricSecurityKey(bytes);
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        private IEnumerable<Claim> CreateClaimsForUser(UserDto dto)
        {
            yield return new Claim(ClaimTypes.Name, dto.Login);

            foreach(var role in dto.Roles)
            {
                yield return new Claim(ClaimTypes.Role, role);
            }
        }
    }
}
