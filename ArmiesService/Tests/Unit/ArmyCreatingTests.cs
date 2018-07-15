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
using IArmiesRepository = ArmiesDomain.Repositories.Armies.IArmies;
using ArmyDtoOfController = ArmiesService.Controllers.Data.ArmyDto;
using SquadDtoOfController = ArmiesService.Controllers.Data.SquadDto;
using ArmyDtoOfRepository = ArmiesDomain.Repositories.Armies.ArmyDto;
using SquadDtoOfRepository = ArmiesDomain.Repositories.Squads.SquadDto;
using Microsoft.AspNetCore.Mvc;
using ArmiesDomain.Repositories.Weapons;
using FluentAssertions;
using ArmiesDomain.Repositories.Armors;
using ArmiesDomain.Repositories.Squads;
using ArmiesDomain.Exceptions;
using ArmiesService.Users;

namespace Tests.Unit
{
    public class ArmyCreatingTests
    {
        [Fact]
        public async Task CreateArmy_FromControllerData_ArmyIsSavedToRepositoryWithExpectedFields()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<ArmiesController>();
            var mockOfArmiesRepository = services.GetService<Mock<IArmiesRepository>>();
            mockOfArmiesRepository.Setup(repository => repository.SaveAsync(It.IsAny<ArmyDtoOfRepository>()))
                                  .Callback<ArmyDtoOfRepository>(ValidateResult)
                                  .Returns(Task.CompletedTask);
                
            await controller.CreateAsync(new ArmyDtoOfController
            {
                Squads = new[]
                {
                    new SquadDtoOfController
                    {
                        Type = "SquadOne",
                        Quantity = 1,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            void ValidateResult(ArmyDtoOfRepository dto)
            {
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
            var controller = services.GetService<ArmiesController>();

            var result = await controller.CreateAsync(new ArmyDtoOfController
            {
                Squads = new[]
                {
                    new SquadDtoOfController
                    {
                        Type = "SquadOne",
                        Quantity = 2,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task CreateArmy_SquadCreatingWithNonPositiveQuantity_BadRequest(int? quantity)
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<ArmiesController>();

            var result = await controller.CreateAsync(new ArmyDtoOfController
            {
                Squads = new[]
                {
                    new SquadDtoOfController
                    {
                        Type = "SquadOne",
                        Quantity = quantity,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateArmy_SquadCreatingWithUnknownType_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<ArmiesController>();
            var mockOfSquadsRepository = services.GetService<Mock<ISquads>>();
            mockOfSquadsRepository.Setup(mock => mock.GetByTypeAsync(It.IsAny<string>()))
                                  .ThrowsAsync(new EntityNotFoundException());

            var result = await controller.CreateAsync(new ArmyDtoOfController
            {
                Squads = new[]
                {
                    new SquadDtoOfController
                    {
                        Type = "SquadOne",
                        Quantity = 1,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateArmy_SquadCreatingWithUnknownWeaponName_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<ArmiesController>();
            var mockOfWeaponsRepository = services.GetService<Mock<IWeapons>>();
            mockOfWeaponsRepository.Setup(mock => mock.GetByNameAsync(It.IsAny<string>()))
                                   .ThrowsAsync(new EntityNotFoundException());

            var result = await controller.CreateAsync(new ArmyDtoOfController
            {
                Squads = new[]
                {
                    new SquadDtoOfController
                    {
                        Type = "SquadOne",
                        Quantity = 1,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateArmy_SquadCreatingWithUnknownArmorName_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<ArmiesController>();
            var mockOfArmorsRepository = services.GetService<Mock<IArmors>>();
            mockOfArmorsRepository.Setup(mock => mock.GetByNameAsync(It.IsAny<string>()))
                                  .ThrowsAsync(new EntityNotFoundException());

            var result = await controller.CreateAsync(new ArmyDtoOfController
            {
                Squads = new[]
                {
                    new SquadDtoOfController
                    {
                        Type = "SquadOne",
                        Quantity = 1,
                        Weapons = new[] { "WeaponOne" },
                        Armors = new[] { "ArmorOne" }
                    }
                }
            });

            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateArmy_WithoutSquads_BadRequest()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<ArmiesController>();
            var data = new ArmyDtoOfController();

            var nullArrayResult = await controller.CreateAsync(data);
            data.Squads = new SquadDtoOfController[0];
            var emptyArrayResult = await controller.CreateAsync(data);

            Assert.IsAssignableFrom<BadRequestResult>(nullArrayResult);
            Assert.IsAssignableFrom<BadRequestResult>(emptyArrayResult);
        }

        [Fact]
        public async Task CreateArmy_FromValidControllerData_ResultIsOk()
        {
            var services = CreateServiceProvider();
            var controller = services.GetService<ArmiesController>();

            var result = await controller.CreateAsync(new ArmyDtoOfController
            {
                Squads = new SquadDtoOfController[]
                {
                    new SquadDtoOfController
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
            var configuration = new Mock<IConfiguration>();
            var services = new ServiceCollection();
            var startup = new Startup(configuration.Object);
            startup.ConfigureServices(services);
            ConfigureController(services);
            MockArmiesRepository(services);
            MockSquadsRepository(services);
            MockWeaponsRepository(services);
            MockArmorsRepository(services);
            MockUsersRepository(services);
            MockLogs(services);
            MockUsers(services);
            return services.BuildServiceProvider();
        }

        private void ConfigureController(IServiceCollection services)
        {
            services.AddTransient<ArmiesController>();
        }

        private void MockArmiesRepository(IServiceCollection services)
        {
            var repository = new Mock<IArmiesRepository>();
            repository.Setup(mock => mock.SaveAsync(It.IsAny<ArmyDtoOfRepository>()))
                      .Returns(Task.CompletedTask);
            services.AddSingleton(repository.Object)
                    .AddSingleton(repository);
        }

        private void MockSquadsRepository(IServiceCollection services)
        {
            var repository = new Mock<ISquads>();
            var someSquad = new SquadDtoOfRepository
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
            var someWeapon = new WeaponDto
            {
                Name = "WeaponOne",
                Cost = 3,
                Offence = new[]
                {
                    new OffenceDto
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
            var someArmor = new ArmorDto
            {
                Name = "ArmorOne",
                Cost = 2,
                Defence = new[]
                {
                    new DefenceDto
                    {
                        Max = 4,
                        Min = 6,
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
            var someUser = new UserDto
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

        private void MockUsers(IServiceCollection services)
        {
            var getCurrentUserLoginStrategy = new Mock<IGetCurrentUserLoginStrategy>();
            getCurrentUserLoginStrategy.SetReturnsDefault("UserOne");
            services.AddSingleton(getCurrentUserLoginStrategy.Object)
                    .AddSingleton(getCurrentUserLoginStrategy);
        }
    }
}
