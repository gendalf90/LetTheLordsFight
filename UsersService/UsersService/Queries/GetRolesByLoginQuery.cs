using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersService.Users;

namespace UsersService.Queries
{
    class GetRolesByLoginQuery : IQuery
    {
        private readonly string login;
        private readonly UsersContext context;

        public GetRolesByLoginQuery(UsersContext context, string login)
        {
            this.login = login;
            this.context = context;
        }

        public async Task<string> AskAsync()
        {
            var user = await context.Users.AsNoTracking()
                                          .Include(data => data.Roles)
                                          .SingleAsync(data => data.Login == login);
            var result = new JArray(user.Roles.Select(role => role.Value));
            return result.ToString();
        }
    }
}
