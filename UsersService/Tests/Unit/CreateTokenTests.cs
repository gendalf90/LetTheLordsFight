using Microsoft.Extensions.DependencyInjection;
using UsersService.Controllers;
using Xunit;
using UsersService.Extensions;
using Moq;
using Microsoft.Extensions.Configuration;
using IQueryFactory = UsersService.Queries.IFactory;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System;

namespace Tests.Unit
{
    public class CreateTokenTests
    {
        private const string Name = "TestName";
        private const string Role = "TestRole";

        [Fact]
        public async Task GetToken_ItIsOkResult()
        {
            var controller = CreateController();

            var result = await controller.GetTokenAsync();

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetToken_NameFromTokenIsExpected()
        {
            var controller = CreateController();

            var result = await controller.GetTokenAsync();
            var token = GetTokenFromResult(result);

            Assert.NotNull(token);
            Assert.Contains(token.Claims, claim => claim.Type == ClaimTypes.Name);
            Assert.Contains(token.Claims, claim => claim.Value == Name);
        }

        [Fact]
        public async Task GetToken_RoleFromTokenIsExpected()
        {
            var controller = CreateController();

            var result = await controller.GetTokenAsync();
            var token = GetTokenFromResult(result);

            Assert.NotNull(token);
            Assert.Contains(token.Claims, claim => claim.Type == ClaimTypes.Role);
            Assert.Contains(token.Claims, claim => claim.Value == Role);
        }

        private UsersController CreateController()
        {
            var provider = CreateServiceProvider();
            return new UsersController(null, provider.GetService<IQueryFactory>());
        }

        private IServiceProvider CreateServiceProvider()
        {
            var configuration = MockConfiguration();
            var httpContextAccessor = MockHttpContextAccessor();
            var services = new ServiceCollection();
            services.AddAuthentication(configuration);
            services.AddQueries();
            services.AddSingleton(httpContextAccessor);
            return services.BuildServiceProvider();
        }

        private IConfiguration MockConfiguration()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.SetupGet(c => c[It.IsAny<string>()]).Returns("testtesttesttest");
            return configuration.Object;
        }

        private IHttpContextAccessor MockHttpContextAccessor()
        {
            var user = MockUser();
            var context = new DefaultHttpContext { User = user };
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.SetupProperty(a => a.HttpContext, context);
            return accessor.Object;
        }

        private ClaimsPrincipal MockUser()
        {
            var claims = new Claim[] { new Claim(ClaimTypes.Name, Name), new Claim(ClaimTypes.Role, Role) };
            var identity = new ClaimsIdentity(claims);
            return new ClaimsPrincipal(identity);
        }

        private JwtSecurityToken GetTokenFromResult(IActionResult result)
        {
            var okResult = result as OkObjectResult;

            if(okResult == null || okResult.Value == null)
            {
                return null;
            }

            var token = okResult.Value.ToString();
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }
    }
}
