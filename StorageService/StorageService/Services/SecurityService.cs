using Microsoft.Extensions.Options;
using StorageService.Options;
using System;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl;

namespace StorageService.Services
{
    class SecurityService : ISecurityService
    {
        private readonly string serviceLogin;
        private readonly string servicePassword;
        private readonly string baseUrl;
        private readonly Lazy<Task<string>> token;

        public SecurityService(IOptions<ApiOptions> apiOptions, IOptions<SecurityOptions> securityOptions)
        {
            serviceLogin = securityOptions.Value.Login;
            servicePassword = securityOptions.Value.Password;
            baseUrl = apiOptions.Value.BaseUri.AbsoluteUri;
            token = new Lazy<Task<string>>(CreateTokenAsync);
        }

        public async Task<string> GetServiceTokenAsync()
        {
            return await token.Value;
        }

        private async Task<string> CreateTokenAsync()
        {
            return await new Url(baseUrl).AppendPathSegment("api/v1/users/current/token")
                                         .WithBasicAuth(serviceLogin, servicePassword)
                                         .GetStringAsync();
        }
    }
}
