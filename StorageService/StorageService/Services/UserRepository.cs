using StorageDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageDomain.Entities;
using StorageService.Services;
using Microsoft.AspNetCore.Http;
using StorageService.Options;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StorageService.Services
{
    class UserRepository : IUsersRepository
    {
        private readonly IHttpContextAccessor contextAccessor;

        public UserRepository(IHttpContextAccessor contextAccessor)
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
