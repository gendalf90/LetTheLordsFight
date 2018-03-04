using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using UsersService.Options;
using TokenQuery = UsersService.Queries.GetCurrentToken.Query;

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

        public IQuery<string> CreateGetTokenQuery()
        {
            return new TokenQuery(jwtOptions, httpContext);
        }
    }
}
