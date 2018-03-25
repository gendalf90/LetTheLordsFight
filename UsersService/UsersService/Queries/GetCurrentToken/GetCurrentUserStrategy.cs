using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace UsersService.Queries.GetCurrentToken
{
    class GetCurrentUserStrategy : IGetCurrentUserStrategy
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private ClaimsPrincipal user;
        private string name;
        private List<string> roles;

        public GetCurrentUserStrategy(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public UserDto Get()
        {
            Initialize();
            FillName();
            FillRoles();
            return CreateDto();
        }

        private void Initialize()
        {
            user = httpContextAccessor.HttpContext.User;
        }

        private void FillName()
        {
            name = user.Identity.Name;
        }

        private void FillRoles()
        {
            roles = user.Claims.Where(claim => claim.Type == ClaimTypes.Role)
                               .Select(claim => claim.Value)
                               .ToList();
        }

        private UserDto CreateDto()
        {
            return new UserDto
            {
                Login = name,
                Roles = roles
            };
        }
    }
}
