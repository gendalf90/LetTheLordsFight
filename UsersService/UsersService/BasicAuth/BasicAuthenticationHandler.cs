using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UsersService.Controllers;

namespace UsersService.BasicAuth
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        public const string HeaderName = "Authorization";

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string header = Request.Headers[HeaderName];
            if (string.IsNullOrEmpty(header))
            {
                return Task.FromResult(AuthenticateResult.Fail($"{HeaderName} header is missing"));
            }

            var token = header.Substring(5).Trim();
            var credentialsString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var credentials = credentialsString.Split(':');

            if (credentials.Length == 2)
            {
                if(credentials[0] == "test" && credentials[1] == "test")
                {
                    var user = new { Login = credentials[0] };
                    var userJson = JsonConvert.SerializeObject(user);
                    var userDataClaim = new Claim(ClaimTypes.UserData, userJson);

                    var identity = new ClaimsIdentity("Basic");
                    identity.AddClaim(userDataClaim);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, null, "Basic");
                    
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
            }

            return Task.FromResult(AuthenticateResult.Fail("Bad credentials"));
        }
    }
}
