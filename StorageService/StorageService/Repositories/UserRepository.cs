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

namespace StorageService.Repositories
{
    class UserRepository : IUsersRepository
    {
        private readonly IUsersService usersService;

        public UserRepository(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<UserEntity> GetCurrentAsync()
        {
            var dto = await usersService.GetCurrentAsync();
            return new UserEntity(dto.Type, dto.StorageId);
        }
    }
}
