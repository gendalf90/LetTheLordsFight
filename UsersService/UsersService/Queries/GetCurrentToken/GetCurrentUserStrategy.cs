using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace UsersService.Queries.GetCurrentToken
{
    class GetCurrentUserStrategy : IGetCurrentUserStrategy
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetCurrentUserStrategy(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public UserDto Get()
        {
            var httpContextUser = httpContextAccessor.HttpContext.User;
            var httpContextLogin = httpContextUser.FindFirst(ClaimTypes.Name);
            var httpContextRoles = httpContextUser.FindAll(ClaimTypes.Role);

            return new UserDto
            {
                Login = httpContextLogin.Value,
                Roles = httpContextRoles.Select(role => role.Value).ToArray()
            };
        }
    }
}
