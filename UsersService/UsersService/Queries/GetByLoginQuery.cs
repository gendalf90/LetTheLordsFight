using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersService.Users;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace UsersService.Queries
{
    class GetByLoginQuery : IQuery
    {
        private readonly string login;
        private readonly UsersContext context;

        public GetByLoginQuery(UsersContext context, string login)
        {
            this.login = login;
            this.context = context;
        }

        public async Task<string> AskAsync()
        {
            var user = await context.Users.AsNoTracking()
                                          .Include(data => data.Roles)
                                          .SingleAsync(data => data.Login == login);
            var result = new
            {
                Login = user.Login,
                Roles = user.Roles.Select(role => role.Value)
            };
            return JsonConvert.SerializeObject(result);
        }
    }
}
