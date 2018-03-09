﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using UsersDomain.Exceptions.Registration;
using UsersDomain.Repositories.Registration;
using UsersDomain.Services.Registration;
using ICommandFactory = UsersService.Commands.IFactory;
using UsersService.Common;
using UsersService.Controllers;
using Xunit;

namespace Tests.Unit
{
    public class CreateRegistrationRequestTests
    {
        private const string TestLogin = "test@test.com";
        private const string TestPassword = "1q2w3e4r!";

        [Theory]
        [InlineData("yandex@yandex.ru")]
        [InlineData("google@gmail.com")]
        [InlineData("foo-bar.baz@example.com")]
        [InlineData("asdfasdfasdfassdfasdfsdfadfasdfasdfasdfasdfasdfasdfasddasdfasdfasdfasdfddddasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfsadfasdfasdfasdfasdfasdfasdfsadfasdfasdfasdfasdfafdadfasdfasdfafasdfasdfasdfas@aasdfadfasdfasdfaasdfasdfasdfaasdfsddsfsdfs.ru")]
        public async Task Login_IsEmail_Ok(string login)
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = login, Password = TestPassword };

            var result = await controller.CreateRegistrationRequestAsync(data);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Theory]
        [InlineData("asdf")]
        [InlineData("qwer.com")]
        [InlineData("qwer@com")]
        [InlineData("asdf:qwer@mail.ru")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("asdfasdfasdfassdfasdfsdfadfasdfasdfasdfasdfasdfasdfasddasdfasdfasdfasdfddddasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfsadfasdfasdfasdfasdfasdfasdfsadfasdfasdfasdfasdfafdadfasdfasdfafasdfasdfasdfas@aasdfadfasdfasdfaasdfasdfasdfaasdfsddsfsdfsa.ru")]
        public async Task Login_IsNotEmail_BadRequest(string login)
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = login, Password = TestPassword };

            var result = await controller.CreateRegistrationRequestAsync(data);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("asd__123")]
        [InlineData("000as+df000")]
        [InlineData("<password&1234>")]
        public async Task Password_IsValid_Ok(string password)
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = password };

            var result = await controller.CreateRegistrationRequestAsync(data);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1+a")]
        [InlineData("ass!76y")]
        public async Task Password_LessThanEightSymbolsOrEmpty_BadRequest(string password)
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = password };

            var result = await controller.CreateRegistrationRequestAsync(data);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("1234567a!0123456")]
        [InlineData("asdf1234asdf78+=")]
        public async Task Password_MoreThanFifteenSymbols_BadRequest(string password)
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = password };

            var result = await controller.CreateRegistrationRequestAsync(data);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("asdf_asdf")]
        [InlineData("!_urul,_l")]
        public async Task Password_NoDigits_BadRequest(string password)
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = password };

            var result = await controller.CreateRegistrationRequestAsync(data);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("!_!+=^&*$")]
        [InlineData("5465^5432")]
        public async Task Password_NoLetters_BadRequest(string password)
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = password };

            var result = await controller.CreateRegistrationRequestAsync(data);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("asdf1234")]
        [InlineData("90as90df")]
        public async Task Password_NoSpecialSymbols_BadRequest(string password)
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = password };

            var result = await controller.CreateRegistrationRequestAsync(data);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Password_HasColon_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = "(90asdf:90qwer)" };

            var result = await controller.CreateRegistrationRequestAsync(data);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Request_Valid_CallSaveToRepositoryWithCredentialsFromInputData()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = TestPassword };
            var mockOfRequestsRepository = services.GetService<Mock<IRequests>>();

            await controller.CreateRegistrationRequestAsync(data);

            mockOfRequestsRepository.Verify(repository => repository.SaveAsync(It.Is<RequestDto>(dto => dto.Login == data.Login && dto.Password == data.Password)), Times.Once);
        }

        [Fact]
        public async Task Request_Valid_CallSaveToRepositoryWithNoEmptyId()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = TestPassword };
            var mockOfRequestsRepository = services.GetService<Mock<IRequests>>();

            await controller.CreateRegistrationRequestAsync(data);

            mockOfRequestsRepository.Verify(repository => repository.SaveAsync(It.Is<RequestDto>(dto => dto.Id != Guid.Empty)), Times.Once);
        }

        [Fact]
        public async Task Request_Valid_CallSaveToRepositoryWith24HoursTTL()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = TestPassword };
            var mockOfRequestsRepository = services.GetService<Mock<IRequests>>();

            await controller.CreateRegistrationRequestAsync(data);

            mockOfRequestsRepository.Verify(repository => repository.SaveAsync(It.Is<RequestDto>(dto => dto.TTL == TimeSpan.FromHours(24))), Times.Once);
        }

        [Fact]
        public async Task Request_ThrowRequestExceptionWithinSaveToRepository_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = TestPassword };
            var mockOfRequestsRepository = services.GetService<Mock<IRequests>>();
            mockOfRequestsRepository.Setup(repository => repository.SaveAsync(It.IsAny<RequestDto>())).Throws<RequestException>();

            var result = await controller.CreateRegistrationRequestAsync(data);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Email_RequestSavedToRepository_CallSendWitAddressIsTheSameAsTheLogin()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = TestPassword };
            var mockOfEmailService = services.GetService<Mock<IEmail>>();

            await controller.CreateRegistrationRequestAsync(data);

            mockOfEmailService.Verify(service => service.SendAsync(It.Is<EmailDto>(dto => dto.Address == data.Login)), Times.Once);
        }

        [Fact]
        public async Task Email_RequestSavedToRepository_CallSendWitNotNullOrEmptyHead()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = TestPassword };
            var mockOfEmailService = services.GetService<Mock<IEmail>>();

            await controller.CreateRegistrationRequestAsync(data);

            mockOfEmailService.Verify(service => service.SendAsync(It.Is<EmailDto>(dto => !string.IsNullOrEmpty(dto.Head))), Times.Once);
        }

        [Fact]
        public async Task Email_RequestSavedToRepository_CallSendWitNotNullOrEmptyBody()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = TestPassword };
            var mockOfEmailService = services.GetService<Mock<IEmail>>();

            await controller.CreateRegistrationRequestAsync(data);

            mockOfEmailService.Verify(service => service.SendAsync(It.Is<EmailDto>(dto => !string.IsNullOrEmpty(dto.Body))), Times.Once);
        }

        [Fact]
        public async Task Email_ThrowRequestExceptionWithinSaveToRequestRepository_DoesNotSend()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = TestPassword };
            var mockOfRequestsRepository = services.GetService<Mock<IRequests>>();
            mockOfRequestsRepository.Setup(repository => repository.SaveAsync(It.IsAny<RequestDto>())).Throws<RequestException>();
            var mockOfEmailService = services.GetService<Mock<IEmail>>();

            await controller.CreateRegistrationRequestAsync(data);

            mockOfEmailService.Verify(service => service.SendAsync(It.IsAny<EmailDto>()), Times.Never);
        }

        [Fact]
        public async Task Registration_Valid_Ok()
        {
            var services = CreateServiceProvider();
            var controller = new UsersController(services.GetService<ICommandFactory>(), null);
            var data = new RegistrationData { Login = TestLogin, Password = TestPassword };

            var result = await controller.CreateRegistrationRequestAsync(data);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        private IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            MockRequestsRepository(services);
            MockEmailService(services);
            return services.BuildServiceProvider();
        }

        private void MockRequestsRepository(ServiceCollection services)
        {
            var repository = new Mock<IRequests>();
            repository.Setup(mock => mock.SaveAsync(It.IsAny<RequestDto>())).Returns(Task.CompletedTask);
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockEmailService(ServiceCollection services)
        {
            var service = new Mock<IEmail>();
            service.Setup(mock => mock.SendAsync(It.IsAny<EmailDto>())).Returns(Task.CompletedTask);
            services.AddSingleton(service.Object)
                    .AddSingleton(service);
        }
    }
}
