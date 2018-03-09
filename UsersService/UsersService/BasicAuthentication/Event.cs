using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace UsersService.BasicAuthentication
{
    public class Event : BasicAuthenticationEvents
    {
        private readonly IGetUserByLoginAndPasswordStrategy getUserStrategy;

        public Event(IGetUserByLoginAndPasswordStrategy getUserStrategy)
        {
            this.getUserStrategy = getUserStrategy;
        }

        public override async Task ValidatePrincipalAsync(ValidatePrincipalContext context)
        {
            var user = await getUserStrategy.GetAsync(context.UserName, context.Password);

            if(user != null)
            {
                context.Principal = CreatePrincipal(user);
            }
        }

        private ClaimsPrincipal CreatePrincipal(UserDto user)
        {
            var claims = CreateClaims(user);
            var identity = new ClaimsIdentity(claims, BasicAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        private IEnumerable<Claim> CreateClaims(UserDto user)
        {
            yield return new Claim(ClaimTypes.Name, user.Login);

            foreach(var role in user.Roles)
            {
                yield return new Claim(ClaimTypes.Role, role);
            }
        }
    }
}
