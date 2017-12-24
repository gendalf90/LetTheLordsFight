using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UsersService.Queries.GetCurrentUser
{
    class Query : IQuery<User>
    {
        private readonly IHttpContextAccessor context;

        public Query(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public Task<User> AskAsync()
        {
            var httpContextUser = context.HttpContext.User;
            var httpContextLogin = httpContextUser.FindFirst(ClaimTypes.Name);
            var httpContextRoles = httpContextUser.FindAll(ClaimTypes.Role);

            var resultUser = new User
            {
                Login = httpContextLogin.Value,
                Roles = httpContextRoles.Select(role => role.Value).ToArray()
            };

            return Task.FromResult(resultUser);
        }
    }
}
