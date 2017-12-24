using MapDomain.Repositories;
using Microsoft.AspNetCore.Http;
using MapDomain.ValueObjects;

namespace MapService.Repositories
{
    class UsersRepository : IUsersRepository
    {
        private readonly IHttpContextAccessor contextAccessor;

        public UsersRepository(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public User GetCurrent()
        {
            var user = contextAccessor.HttpContext.User;
            var isSystem = user.IsInRole("System");
            var login = user.Identity.Name;
            return new User(login, isSystem);
        }
    }
}
