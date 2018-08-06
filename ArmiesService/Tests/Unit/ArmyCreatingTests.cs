using ArmiesDomain.Repositories.Users;
using ArmiesService;
using ArmiesService.Controllers;
using ArmiesService.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using SquadDtoOfSquadsRepository = ArmiesDomain.Repositories.Squads.SquadRepositoryDto;
using Microsoft.AspNetCore.Mvc;
using ArmiesDomain.Repositories.Weapons;
using FluentAssertions;
using ArmiesDomain.Repositories.Armors;
using ArmiesDomain.Repositories.Squads;
using ArmiesDomain.Exceptions;
using ArmiesService.Common;
using ArmiesDomain.Services.ArmyNotifications;
using ArmiesDomain.Repositories.Armies;
using ArmiesService.Controllers.Data;
using ArmiesService.Initialization;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace Tests.Unit
{
    public class ArmyCreatingTests
    {
        [Fact]
        public async Task CreateArmy_FromControllerData_ArmyIsSavedToRepositoryWithExpectedFields()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<CreateArmyController>();
            var mockOfArmiesRepository = services.GetService<Mock<IArmies>>();
            mockOfArmiesRepository.Setup(repository => repository.SaveAsync(It.IsAny<ArmyRepositoryDto>()))
                                  .Callback<ArmyRepositoryDto>(ValidateResult)
                                  .Returns(Task.CompletedTask);
                
            await controller.CreateAsync(new ArmyControllerDto
            {
                Squads = new[]
                {
                    new SquadContollerDto
                    {
                        Type = "SquadOne",
                        Quantity = 1,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            void ValidateResult(ArmyRepositoryDto dto)
            {
                dto.OwnerLogin.Should()
                              .BeSameAs("UserOne");

                dto.Squads.Should()
                          .ContainSingle()
                          .Which
                          .Should()
                          .BeEquivalentTo(new
                          {
                              Type = "SquadOne",
                              Quantity = 1,
                              Weapons = new[] { "WeaponOne" },
                              Armors = new[] { "ArmorOne" }
                          }, options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task CreateArmy_CostForUserIsExceeded_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<CreateArmyController>();

            var result = await controller.CreateAsync(new ArmyControllerDto
            {
                Squads = new[]
                {
                    new SquadContollerDto
                    {
                        Type = "SquadOne",
                        Quantity = 2,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task CreateArmy_SquadCreatingWithNonPositiveQuantity_BadRequest(int? quantity)
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<CreateArmyController>();

            var result = await controller.CreateAsync(new ArmyControllerDto
            {
                Squads = new[]
                {
                    new SquadContollerDto
                    {
                        Type = "SquadOne",
                        Quantity = quantity,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateArmy_SquadCreatingWithUnknownType_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<CreateArmyController>();
            var mockOfSquadsRepository = services.GetService<Mock<ISquads>>();
            mockOfSquadsRepository.Setup(mock => mock.GetByTypeAsync(It.IsAny<string>()))
                                  .ThrowsAsync(new EntityNotFoundException());

            var result = await controller.CreateAsync(new ArmyControllerDto
            {
                Squads = new[]
                {
                    new SquadContollerDto
                    {
                        Type = "SquadOne",
                        Quantity = 1,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateArmy_SquadCreatingWithUnknownWeaponName_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<CreateArmyController>();
            var mockOfWeaponsRepository = services.GetService<Mock<IWeapons>>();
            mockOfWeaponsRepository.Setup(mock => mock.GetByNameAsync(It.IsAny<string>()))
                                   .ThrowsAsync(new EntityNotFoundException());

            var result = await controller.CreateAsync(new ArmyControllerDto
            {
                Squads = new[]
                {
                    new SquadContollerDto
                    {
                        Type = "SquadOne",
                        Quantity = 1,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateArmy_SquadCreatingWithUnknownArmorName_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<CreateArmyController>();
            var mockOfArmorsRepository = services.GetService<Mock<IArmors>>();
            mockOfArmorsRepository.Setup(mock => mock.GetByNameAsync(It.IsAny<string>()))
                                  .ThrowsAsync(new EntityNotFoundException());

            var result = await controller.CreateAsync(new ArmyControllerDto
            {
                Squads = new[]
                {
                    new SquadContollerDto
                    {
                        Type = "SquadOne",
                        Quantity = 1,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateArmy_WithoutSquads_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<CreateArmyController>();
            var data = new ArmyControllerDto();

            var nullArrayResult = await controller.CreateAsync(data);
            data.Squads = new SquadContollerDto[0];
            var emptyArrayResult = await controller.CreateAsync(data);

            Assert.IsAssignableFrom<BadRequestObjectResult>(nullArrayResult);
            Assert.IsAssignableFrom<BadRequestObjectResult>(emptyArrayResult);
        }

        [Fact]
        public async Task CreateArmy_FromControllerData_NotificationIsSentWithExpectedFields()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<CreateArmyController>();
            var data = new ArmyControllerDto();
            var mockOfArmyNotificationsService = services.GetService<Mock<IArmyNotificationService>>();
            mockOfArmyNotificationsService.Setup(service => service.NotifyThatCreatedAsync(It.IsAny<ArmyNotificationDto>()))
                                          .Callback<ArmyNotificationDto>(ValidateResult)
                                          .Returns(Task.CompletedTask);

            await controller.CreateAsync(new ArmyControllerDto
            {
                Squads = new[]
                {
                    new SquadContollerDto
                    {
                        Type = "SquadOne",
                        Quantity = 1,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            void ValidateResult(ArmyNotificationDto dto)
            {
                dto.OwnerLogin.Should()
                              .BeSameAs("UserOne");

                dto.Squads.Should()
                          .ContainSingle()
                          .Which
                          .Should()
                          .BeEquivalentTo(new
                          {
                              Type = "SquadOne",
                              Quantity = 1,
                              Weapons = new
                              {
                                  Name = "WeaponOne",
                                  Offence = new[]
                                  {
                                      new
                                      {
                                          Max = 10,
                                          Min = 1,
                                          Tags = new[] { "TagOne" }
                                      }
                                  },
                                  Tags = new[] { "TagTwo" }
                              },
                              Armors = new
                              {
                                  Name = "ArmorOne",
                                  Cost = 2,
                                  Defence = new[]
                                  {
                                      new
                                      {
                                          Max = 6,
                                          Min = 4,
                                          Tags = new[] { "TagOne" }
                                      }
                                  },
                                  Tags = new[] { "TagTwo" }
                              },
                              Tags = new[] { "TagOne" }
                          }, options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task CreateArmy_FromValidControllerData_ResultIsOk()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<CreateArmyController>();

            var result = await controller.CreateAsync(new ArmyControllerDto
            {
                Squads = new SquadContollerDto[]
                {
                    new SquadContollerDto
                    {
                        Type = "SquadOne",
                        Quantity = 1,
                        Armors = new[] { "ArmorOne" },
                        Weapons = new[] { "WeaponOne" }
                    }
                }
            });

            Assert.IsAssignableFrom<OkResult>(result);
        }

        private IServiceProvider CreateServiceProvider()
        {
            var configuration = GetConfiguration();
            var services = new ServiceCollection().AddCommands()
                                                  .AddDomain();
            ConfigureController(services);
            MockArmiesRepository(services);
            MockSquadsRepository(services);
            MockWeaponsRepository(services);
            MockArmorsRepository(services);
            MockUsersRepository(services);
            MockArmyNotificationsService(services);
            MockLogs(services);
            MockCommons(services);
            return services.BuildServiceProvider();
        }

        private IConfiguration GetConfiguration()
        {
            var section = new Mock<IConfigurationSection>();
            section.SetReturnsDefault(section.Object);
            return section.Object;
        }

        private void ConfigureController(IServiceCollection services)
        {
            services.AddTransient<CreateArmyController>();
        }

        private void MockArmiesRepository(IServiceCollection services)
        {
            var repository = new Mock<IArmies>();
            repository.Setup(mock => mock.SaveAsync(It.IsAny<ArmyRepositoryDto>()))
                      .Returns(Task.CompletedTask);
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockSquadsRepository(IServiceCollection services)
        {
            var repository = new Mock<ISquads>();
            var someSquad = new SquadDtoOfSquadsRepository
            {
                Type = "SquadOne",
                Cost = 5,
                Tags = new[] { "TagOne" }
            };
            repository.Setup(mock => mock.GetByTypeAsync(It.IsAny<string>()))
                      .Returns(Task.FromResult(someSquad));
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockWeaponsRepository(IServiceCollection services)
        {
            var repository = new Mock<IWeapons>();
            var someWeapon = new WeaponRepositoryDto
            {
                Name = "WeaponOne",
                Cost = 3,
                Offence = new[]
                {
                    new OffenceRepositoryDto
                    {
                        Max = 10,
                        Min = 1,
                        Tags = new[] { "TagOne" }
                    }
                },
                Tags = new[] { "TagTwo" }
            };
            repository.Setup(mock => mock.GetByNameAsync(It.IsAny<string>()))
                      .Returns(Task.FromResult(someWeapon));
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockArmorsRepository(IServiceCollection services)
        {
            var repository = new Mock<IArmors>();
            var someArmor = new ArmorRepositoryDto
            {
                Name = "ArmorOne",
                Cost = 2,
                Defence = new[]
                {
                    new DefenceRepositoryDto
                    {
                        Max = 6,
                        Min = 4,
                        Tags = new[] { "TagOne" }
                    }
                },
                Tags = new[] { "TagTwo" }
            };
            repository.Setup(mock => mock.GetByNameAsync(It.IsAny<string>()))
                      .Returns(Task.FromResult(someArmor));
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockUsersRepository(IServiceCollection services)
        {
            var repository = new Mock<IUsers>();
            var someUser = new UserRepositoryDto
            {
                Login = "UserOne",
                ArmyCostLimit = 10
            };
            repository.Setup(mock => mock.GetByLoginAsync(It.IsAny<string>()))
                      .Returns(Task.FromResult(someUser));
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockLogs(IServiceCollection services)
        {
            var log = new Mock<ILog>();
            services.AddSingleton(log.Object);
        }

        private void MockCommons(IServiceCollection services)
        {
            var getCurrentUserLoginStrategy = new Mock<IGetCurrentUserLoginStrategy>();
            getCurrentUserLoginStrategy.SetReturnsDefault("UserOne");
            services.AddSingleton(getCurrentUserLoginStrategy.Object)
                    .AddSingleton(getCurrentUserLoginStrategy);
        }

        private void MockArmyNotificationsService(IServiceCollection services)
        {
            var service = new Mock<IArmyNotificationService>();
            services.AddSingleton(service.Object)
                    .AddSingleton(service);
        }
    }
}
