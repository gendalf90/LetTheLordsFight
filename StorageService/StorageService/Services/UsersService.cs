using Microsoft.Extensions.Options;
using StorageService.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using StorageDomain.Repositories;
using StorageDomain.Entities;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Flurl;
using Flurl.Http;

namespace StorageService.Services
{
    class UsersService : IUsersService
    {
        private readonly ISecurityService securityService;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly string baseUrl;

        public UsersService(IOptions<ApiOptions> options, IHttpContextAccessor contextAccessor, ISecurityService securityService)
        {
            this.securityService = securityService;
            this.contextAccessor = contextAccessor;
            baseUrl = options.Value.BaseUri.AbsoluteUri;
        }

        public async Task<UserDto> GetByStorageIdAsync(string storageId)
        {
            var current = await GetCurrentAsync();

            if(current.StorageId == storageId)
            {
                return current;
            }

            var serviceAuthToken = await securityService.GetServiceTokenAsync();

            return await new Url(baseUrl).AppendPathSegment("api/v1/users")
                                         .SetQueryParam("storageid", storageId)
                                         .WithOAuthBearerToken(serviceAuthToken)
                                         .GetJsonAsync<UserDto>();
        }

        public async Task<UserDto> GetCurrentAsync()
        {
            var userDataClaim = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.UserData);
            var dto = GetFromJson(userDataClaim.Value);
            return await Task.FromResult(dto);
        }

        private UserDto GetFromJson(string json)
        {
            return JsonConvert.DeserializeObject<UserDto>(json);
        }
    }
}
