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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using UsersService.Users;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UsersService.Authentication.Basic
{
    class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private const string HeaderName = "Authorization";

        private readonly UsersContext context;

        private string header;
        private string[] credentials;
        private UserData user;
        private AuthenticationTicket ticket;

        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options, 
                                          ILoggerFactory logger, 
                                          UrlEncoder encoder, 
                                          ISystemClock clock,
                                          UsersContext context) 
            : base(options, logger, encoder, clock)
        {
            this.context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            LoadHeader();

            if(HasNoHeader)
            {
                return HeaderNotFoundResult;
            }

            LoadCredentials();

            if(IsCredentialsNotValid)
            {
                return BadCredentialsResult;
            }

            await LoadUserAsync();

            if(HasNoUser)
            {
                return UserNotFoundResult;
            }

            CreateTicket();
            return SuccessResult;
        }

        private void LoadHeader()
        {
            header = Request.Headers[HeaderName];
        }

        private bool HasNoHeader
        {
            get => string.IsNullOrWhiteSpace(header);
        }

        private void LoadCredentials()
        {
            var token = header.Substring(5).Trim();
            var credentialsString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            credentials = credentialsString.Split(':');
        }

        private bool IsCredentialsNotValid
        {
            get => credentials.Length != 2;
        }

        private async Task LoadUserAsync()
        {
            user = await context.Users.SingleOrDefaultAsync(data => data.Login == credentials[0] && data.Password == credentials[1]);
        }

        private bool HasNoUser
        {
            get => user == null;
        }

        private void CreateTicket()
        {
            var claims = GetUserClaims();
            var identity = new ClaimsIdentity(claims, BasicDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            ticket = new AuthenticationTicket(principal, BasicDefaults.AuthenticationScheme);
        }

        private IEnumerable<Claim> GetUserClaims()
        {
            yield return new Claim(ClaimTypes.Name, user.Login);

            foreach(var role in user.Roles)
            {
                yield return new Claim(ClaimTypes.Role, role.Value);
            }
        }

        private AuthenticateResult HeaderNotFoundResult
        {
            get => AuthenticateResult.Fail($"{HeaderName} header is missing");
        }

        private AuthenticateResult BadCredentialsResult
        {
            get => AuthenticateResult.Fail("Bad credentials");
        }

        private AuthenticateResult UserNotFoundResult
        {
            get => AuthenticateResult.Fail("User not found");
        }

        private AuthenticateResult SuccessResult
        {
            get => AuthenticateResult.Success(ticket);
        }
    }
}
