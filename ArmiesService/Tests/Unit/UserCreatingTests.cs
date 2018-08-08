using ArmiesDomain.Repositories.Users;
using ArmiesService.Commands;
using ArmiesService.Logs;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using ArmiesService.Consumers.Data;
using ArmiesService.Initialization;
using ArmiesDomain.Repositories.Armies;
using ArmiesService.Common;
using ArmiesDomain.Services.ArmyNotifications;
using ArmiesDomain.Factories.Armies;

namespace Tests.Unit
{
    public class UserCreatingTests
    {
        [Fact]
        public async Task CreateUser_FromConsumerData_UserIsSavedToRepositoryWithExpectedFields()
        {
            var services = CreateServiceProvider();
            var commandFactory = services.GetService<ICommandsFactory>();
            var mockOfUsersRepository = services.GetService<Mock<IUsers>>();
            mockOfUsersRepository.Setup(repository => repository.SaveAsync(It.IsAny<UserRepositoryDto>()))
                                 .Callback<UserRepositoryDto>(ValidateResult)
                                 .Returns(Task.CompletedTask);

            var command = commandFactory.GetCreateUserCommand(new UserCreatedEventDto
            {
                Login = "TestLogin"
            });
            await command.ExecuteAsync();

            void ValidateResult(UserRepositoryDto dto)
            {
                dto.Should().BeEquivalentTo(new
                {
                    ArmyCostLimit = 50,
                    Login = "TestLogin"
                }, options => options.ExcludingMissingMembers());
            }
        }

        private IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection().AddDomain()
                                                  .AddCommands();
            MockUsersRepository(services);
            MockLogs(services);
            MockOtherDependencies(services);
            return services.BuildServiceProvider();
        }

        private void MockUsersRepository(IServiceCollection services)
        {
            var repository = new Mock<IUsers>();
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockLogs(IServiceCollection services)
        {
            var log = new Mock<ILog>();
            services.AddSingleton(log.Object);
        }

        private void MockOtherDependencies(IServiceCollection services)
        {
            var armies = new Mock<IArmies>();
            services.AddSingleton(armies.Object);
            var armyFactory = new Mock<IArmyFactory>();
            services.AddSingleton(armyFactory.Object);
            var strategy = new Mock<IGetCurrentUserLoginStrategy>();
            services.AddSingleton(strategy.Object);
            var notifications = new Mock<IArmyNotificationService>();
            services.AddSingleton(notifications.Object);
        }
    }
}
