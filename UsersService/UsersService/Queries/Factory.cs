using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using UsersService.Options;
using CurrentUserQuery = UsersService.Queries.GetCurrentUser.Query;
using UserByLoginQuery = UsersService.Queries.GetUserByLogin.Query;
using TokenQuery = UsersService.Queries.GetCurrentToken.Query;
using LoginUser = UsersService.Queries.GetUserByLogin.User;
using CurrentUser = UsersService.Queries.GetCurrentUser.User;

namespace UsersService.Queries
{
    class Factory : IFactory
    {
        private readonly IOptions<SqlOptions> sqlOptions;
        private readonly IHttpContextAccessor httpContext;
        private readonly IOptions<JwtOptions> jwtOptions;

        public Factory(IOptions<SqlOptions> sqlOptions, IHttpContextAccessor httpContext, IOptions<JwtOptions> jwtOptions)
        {
            this.sqlOptions = sqlOptions;
            this.httpContext = httpContext;
            this.jwtOptions = jwtOptions;
        }

        public IQuery<CurrentUser> CreateGetCurrentUserQuery()
        {
            return new CurrentUserQuery(httpContext);
        }

        public IQuery<string> CreateGetTokenQuery()
        {
            return new TokenQuery(jwtOptions, httpContext);
        }

        public IQuery<LoginUser> CreateGetUserByLoginQuery(string login)
        {
            return new UserByLoginQuery(sqlOptions, login);
        }
    }
}
