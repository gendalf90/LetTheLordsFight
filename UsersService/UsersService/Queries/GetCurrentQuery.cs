using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UsersService.Queries
{
    class GetCurrentQuery : IQuery
    {
        private readonly IHttpContextAccessor context;

        public GetCurrentQuery(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public Task<string> AskAsync()
        {
            var user = context.HttpContext.User;
            var login = user.FindFirst(ClaimTypes.Name);
            var roles = user.FindAll(ClaimTypes.Role);
            var result = new { Login = login.Value, Roles = roles.Select(role => role.Value) };
            var json = JsonConvert.SerializeObject(result);
            return Task.FromResult(json);
        }
    }
}
