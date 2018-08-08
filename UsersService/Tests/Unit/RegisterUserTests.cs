using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using UsersDomain.Exceptions;
using UsersDomain.Exceptions.Registration;
using UsersDomain.Repositories;
using UsersDomain.Repositories.Registration;
using UsersService.Controllers;
using Xunit;
using UsersService.Initialization;
using UsersService.Logs;
using Microsoft.Extensions.Configuration;
using UsersDomain.Services.Registration;
using UserRepositoryDto = UsersDomain.Repositories.UserDto;
using UserNotificationDto = UsersDomain.Services.Registration.UserDto;

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
            var controller = services.GetService<UsersController>();
            var mockOfRequestsRepository = services.GetService<Mock<IRequests>>();
            mockOfRequestsRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>())).Throws<RequestException>();

            var result = await controller.RegisterByRequestIdAsync(Guid.Empty);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task User_ThrowRequestExceptionWithinRequestGet_DoesNotSave()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<UsersController>();
            var mockOfRequestsRepository = services.GetService<Mock<IRequests>>();
            mockOfRequestsRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>())).Throws<RequestException>();
            var mockOfUsersRepository = services.GetService<Mock<IUsers>>();

            await controller.RegisterByRequestIdAsync(Guid.Empty);

            mockOfUsersRepository.Verify(repository => repository.SaveAsync(It.IsAny<UserRepositoryDto>()), Times.Never);
        }

        [Fact]
        public async Task User_RegistrationRequestIsFound_SavedWithNotEmptyId()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<UsersController>();
            var mockOfUsersRepository = services.GetService<Mock<IUsers>>();

            await controller.RegisterByRequestIdAsync(Guid.Empty);

            mockOfUsersRepository.Verify(repository => repository.SaveAsync(It.Is<UserRepositoryDto>(dto => dto.Id != Guid.Empty)), Times.Once);
        }

        [Fact]
        public async Task User_RegistrationRequestIsFound_SavedWithCredentialsFromRequest()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<UsersController>();
            var mockOfUsersRepository = services.GetService<Mock<IUsers>>();

            await controller.RegisterByRequestIdAsync(Guid.Empty);

            mockOfUsersRepository.Verify(repository => repository.SaveAsync(It.Is<UserRepositoryDto>(dto => dto.Login == TestLogin && dto.Password == TestPassword)), Times.Once);
        }

        [Fact]
        public async Task User_ThrowUserExceptionWhenSave_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<UsersController>();
            var mockOfUsersRepository = services.GetService<Mock<IUsers>>();
            mockOfUsersRepository.Setup(repository => repository.SaveAsync(It.IsAny<UserRepositoryDto>())).Throws<UserException>();

            var result = await controller.RegisterByRequestIdAsync(Guid.Empty);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Notification_UserIsSaved_IsSentWithExpectedFields()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<UsersController>();
            var mockOfNotificationService = services.GetService<Mock<INotification>>();

            await controller.RegisterByRequestIdAsync(Guid.Empty);

            mockOfNotificationService.Verify(service => service.NotifyAsync(It.Is<UserNotificationDto>(dto => dto.Login == TestLogin)), Times.Once);
        }

        [Fact]
        public async Task Registration_Valid_Ok()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<UsersController>();

            var result = await controller.RegisterByRequestIdAsync(Guid.Empty);

            Assert.IsAssignableFrom<OkResult>(result);
        }

        private IServiceProvider CreateServiceProvider()
        {
            var configuration = new Mock<IConfiguration>();
            var services = new ServiceCollection().AddCommands(configuration.Object)
                                                  .AddDomain(configuration.Object);
            MockRequestsRepository(services);
            MockUsersRepository(services);
            MockNotificationService(services);
            MockLogs(services);
            ConfigureController(services);
            return services.BuildServiceProvider();
        }

        private void ConfigureController(IServiceCollection services)
        {
            services.AddTransient<UsersController>();
        }

        private void MockRequestsRepository(IServiceCollection services)
        {
            var defaultRequest = new RequestDto { Id = Guid.Empty, Login = TestLogin, Password = TestPassword, TTL = TimeSpan.Zero };
            var repository = new Mock<IRequests>();
            repository.SetReturnsDefault(Task.CompletedTask);
            repository.SetReturnsDefault(Task.FromResult(defaultRequest));
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockUsersRepository(IServiceCollection services)
        {
            var repository = new Mock<IUsers>();
            repository.SetReturnsDefault(Task.CompletedTask);
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockNotificationService(IServiceCollection services)
        {
            var service = new Mock<INotification>();
            service.SetReturnsDefault(Task.CompletedTask);
            services.AddSingleton(service.Object)
                    .AddSingleton(service);
        }

        private void MockLogs(IServiceCollection services)
        {
            var log = new Mock<ILog>();
            services.AddSingleton(log.Object);
        }
    }
}
