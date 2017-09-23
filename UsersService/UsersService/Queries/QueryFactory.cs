using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersService.Options;
using UsersService.Users;

namespace UsersService.Queries
{
    class QueryFactory : IQueryFactory
    {
        private readonly UsersContext usersContext;
        private readonly IHttpContextAccessor httpContext;
        private readonly IOptions<JwtOptions> jwtOptions;

        public QueryFactory(UsersContext usersContext, IHttpContextAccessor httpContext, IOptions<JwtOptions> jwtOptions)
        {
            this.usersContext = usersContext;
            this.httpContext = httpContext;
            this.jwtOptions = jwtOptions;
        }

        public IQuery CreateCurrentQuery()
        {
            return new GetCurrentQuery(httpContext);
        }

        public IQuery CreateLoginQuery(string login)
        {
            return new GetByLoginQuery(usersContext, login);
        }

        public IQuery CreateTokenQuery()
        {
            return new GetTokenQuery(jwtOptions, httpContext);
        }
    }
}
