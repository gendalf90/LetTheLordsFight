using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using UsersService.Options;

namespace UsersService.Queries
{
    class QueryFactory : IQueryFactory
    {
        private readonly IOptions<SqlOptions> sqlOptions;
        private readonly IHttpContextAccessor httpContext;
        private readonly IOptions<JwtOptions> jwtOptions;

        public QueryFactory(IOptions<SqlOptions> sqlOptions, IHttpContextAccessor httpContext, IOptions<JwtOptions> jwtOptions)
        {
            this.sqlOptions = sqlOptions;
            this.httpContext = httpContext;
            this.jwtOptions = jwtOptions;
        }

        public IQuery CreateCurrentQuery()
        {
            return new GetCurrentQuery(httpContext);
        }

        public IQuery CreateLoginQuery(string login)
        {
            return new GetByLoginQuery(sqlOptions, login);
        }

        public IQuery CreateTokenQuery()
        {
            return new GetTokenQuery(jwtOptions, httpContext);
        }
    }
}
