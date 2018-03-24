using Microsoft.Extensions.DependencyInjection;
using UsersService.Controllers;
using Xunit;
using Moq;
using IQueryFactory = UsersService.Queries.IFactory;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System;
using UsersService.Queries.GetCurrentToken;
using System.Linq;
using UsersService.Extensions;

namespace Tests.Unit
{
    public class CreateTokenTests
    {
        private const string TestLogin = "test@test.com";
        private const string TestRole = "role";

        [Fact]
        public async Task GetToken_ItIsOkResult()
        {
            var provider = CreateServiceProvider();
            var controller = new TokensController(provider.GetService<IQueryFactory>());
            var mockOfGetCurrentUserStrategy = provider.GetService<Mock<IGetCurrentUserStrategy>>();
            var userToReturn = new UserDto { Login = TestLogin, Roles = new[] { TestRole } };
            mockOfGetCurrentUserStrategy.Setup(mock => mock.Get()).Returns(userToReturn);

            var result = await controller.GetAsync();

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetToken_NameFromTokenIsEqualLoginFromCurrentUser()
        {
            var provider = CreateServiceProvider();
            var controller = new TokensController(provider.GetService<IQueryFactory>());
            var mockOfGetCurrentUserStrategy = provider.GetService<Mock<IGetCurrentUserStrategy>>();
            var userToReturn = new UserDto { Login = TestLogin, Roles = new[] { TestRole } };
            mockOfGetCurrentUserStrategy.Setup(mock => mock.Get()).Returns(userToReturn);

            var result = await controller.GetAsync();
            var token = GetTokenFromResult(result);

            Assert.NotNull(token);
            Assert.Single(token.Claims, claim => claim.Type == ClaimTypes.Name && claim.Value == userToReturn.Login);
        }

        [Fact]
        public async Task GetToken_RoleFromTokenIsEqualFromCurrentUser()
        {
            var provider = CreateServiceProvider();
            var controller = new TokensController(provider.GetService<IQueryFactory>());
            var mockOfGetCurrentUserStrategy = provider.GetService<Mock<IGetCurrentUserStrategy>>();
            var userToReturn = new UserDto { Login = TestLogin, Roles = new[] { TestRole } };
            mockOfGetCurrentUserStrategy.Setup(mock => mock.Get()).Returns(userToReturn);

            var result = await controller.GetAsync();
            var token = GetTokenFromResult(result);

            Assert.NotNull(token);
            Assert.Contains(token.Claims, claim => claim.Type == ClaimTypes.Role && claim.Value == userToReturn.Roles.Single());
        }

        private IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddQueries();
            MockGetCurrentUserStrategy(services);
            MockGetTokenSigningKeyStrategy(services);
            return services.BuildServiceProvider();
        }

        private void MockGetCurrentUserStrategy(ServiceCollection services)
        {
            var strategy = new Mock<IGetCurrentUserStrategy>();
            services.AddSingleton(strategy)
                    .AddSingleton(strategy.Object);
        }

        private void MockGetTokenSigningKeyStrategy(ServiceCollection services)
        {
            var strategy = new Mock<IGetTokenSigningKeyStrategy>();
            var testKey = "keykeykeykeykeykeykeykey";
            strategy.Setup(mock => mock.Get()).Returns(testKey);
            services.AddSingleton(strategy.Object);
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
