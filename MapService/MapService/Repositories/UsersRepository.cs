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
            var userDataClaim = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.UserData);
            return GetFromJson(userDataClaim.Value);
        }

        private User GetFromJson(string json)
        {
            var data = JObject.Parse(json);
            var type = data.Value<string>("type");
            var mapObjectId = data.Value<string>("mapObjectId");
            return new User(type, mapObjectId);
        }
    }
}
