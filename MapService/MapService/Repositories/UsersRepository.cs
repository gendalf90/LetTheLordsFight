using MapDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapDomain.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
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
