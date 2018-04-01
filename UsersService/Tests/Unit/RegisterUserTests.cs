using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using UsersDomain.Exceptions;
using UsersDomain.Exceptions.Registration;
using UsersDomain.Repositories;
using UsersDomain.Repositories.Registration;
using ICommandFactory = UsersService.Commands.IFactory;
using UsersService.Controllers;
using Xunit;
using UsersService.Initialization;
using UsersService.Logs;
using Microsoft.Extensions.Configuration;
using UsersService;

namespace Tests.Unit
{
    public class RegisterUserTests
    {
        private const string TestLogin = "test@test.com";
        private const string TestPassword = "1q2w3e4r!";

        [Fact]
        public async Task RegistrationRequest_ThrowExceptionWithinGet_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), services.GetService<ILog>());
            var mockOfRequestsRepository = services.GetService<Mock<IRequests>>();
            mockOfRequestsRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>())).Throws<RequestException>();

            var result = await controller.RegisterByRequestIdAsync(Guid.Empty);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task User_ThrowRequestExceptionWithinRequestGet_DoesNotSave()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), services.GetService<ILog>());
            var mockOfRequestsRepository = services.GetService<Mock<IRequests>>();
            mockOfRequestsRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>())).Throws<RequestException>();
            var mockOfUsersRepository = services.GetService<Mock<IUsers>>();

            await controller.RegisterByRequestIdAsync(Guid.Empty);

            mockOfUsersRepository.Verify(repository => repository.SaveAsync(It.IsAny<UserDto>()), Times.Never);
        }

        [Fact]
        public async Task User_RegistrationRequestIsFound_SavedWithNotEmptyId()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), services.GetService<ILog>());
            var mockOfUsersRepository = services.GetService<Mock<IUsers>>();

            await controller.RegisterByRequestIdAsync(Guid.Empty);

            mockOfUsersRepository.Verify(repository => repository.SaveAsync(It.Is<UserDto>(dto => dto.Id != Guid.Empty)), Times.Once);
        }

        [Fact]
        public async Task User_RegistrationRequestIsFound_SavedWithCredentialsFromRequest()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), services.GetService<ILog>());
            var mockOfRequestsRepository = services.GetService<Mock<IRequests>>();
            var requestToReturn = new RequestDto { Login = TestLogin, Password = TestPassword };
            mockOfRequestsRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(requestToReturn));
            var mockOfUsersRepository = services.GetService<Mock<IUsers>>();

            await controller.RegisterByRequestIdAsync(Guid.Empty);

            mockOfUsersRepository.Verify(repository => repository.SaveAsync(It.Is<UserDto>(dto => dto.Login == requestToReturn.Login && dto.Password == requestToReturn.Password)), Times.Once);
        }

        [Fact]
        public async Task User_ThrowUserExceptionWhenSave_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), services.GetService<ILog>());
            var mockOfUsersRepository = services.GetService<Mock<IUsers>>();
            mockOfUsersRepository.Setup(repository => repository.SaveAsync(It.IsAny<UserDto>())).Throws<UserException>();

            var result = await controller.RegisterByRequestIdAsync(Guid.Empty);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Registration_Valid_Ok()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), services.GetService<ILog>());

            var result = await controller.RegisterByRequestIdAsync(Guid.Empty);

            Assert.IsAssignableFrom<OkResult>(result);
        }

        private IServiceProvider CreateServiceProvider()
        {
            var configuration = new Mock<IConfiguration>();
            var services = new ServiceCollection();
            var startup = new Startup(configuration.Object);
            startup.ConfigureServices(services);
            MockRequestsRepository(services);
            MockUsersRepository(services);
            MockLogs(services);
            return services.BuildServiceProvider();
        }

        private void MockRequestsRepository(IServiceCollection services)
        {
            var defaultRequest = new RequestDto { Id = Guid.Empty, Login = TestLogin, Password = TestPassword, TTL = TimeSpan.Zero };
            var repository = new Mock<IRequests>();
            repository.Setup(mock => mock.SaveAsync(It.IsAny<RequestDto>())).Returns(Task.CompletedTask);
            repository.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(defaultRequest));
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockUsersRepository(IServiceCollection services)
        {
            var repository = new Mock<IUsers>();
            repository.Setup(mock => mock.SaveAsync(It.IsAny<UserDto>())).Returns(Task.CompletedTask);
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockLogs(IServiceCollection services)
        {
            var log = new Mock<ILog>();
            services.AddSingleton(log.Object);
        }
    }
}
