using ArmiesDomain.Factories.Armies;
using ArmiesDomain.Repositories.Armies;
using ArmiesService.Logs;
using System.Linq;
using System.Threading.Tasks;
using ArmiesDomain.Entities;
using ArmiesService.Common;
using ArmiesDomain.Services.ArmyNotifications;
using ArmiesService.Controllers.Data;

namespace ArmiesService.Commands.CreateArmy
{
    class CreateArmyCommand : ICommand
    {
        private readonly IArmyFactory armyFactory;
        private readonly IArmies armyRepository;
        private readonly IGetCurrentUserLoginStrategy currentUserLogin;
        private readonly IArmyNotificationService armyNotificationsService;
        private readonly ILog logger;
        private readonly ArmyControllerDto data;

        private ArmyFactoryDto factoryData;
        private Army army;

        public CreateArmyCommand(IArmyFactory armyFactory,
                       IArmies armyRepository,
                       IGetCurrentUserLoginStrategy currentUserLogin,
                       IArmyNotificationService armyNotificationsService,
                       ILog logger,
                       ArmyControllerDto data)
        {
            this.armyFactory = armyFactory;
            this.armyRepository = armyRepository;
            this.currentUserLogin = currentUserLogin;
            this.armyNotificationsService = armyNotificationsService;
            this.logger = logger;
            this.data = data;
        }

        public async Task ExecuteAsync()
        {
            CreateFactoryData();
            await CreateArmyAsync();
            await SaveArmyAsync();
            await NotifyThatCreatedAsync();
            LogThatCreated();
        }

        private void CreateFactoryData()
        {
            factoryData = new ArmyFactoryDto();
            factoryData.OwnerLogin = currentUserLogin.Get();
            factoryData.Squads = data.Squads?
                                     .Select(Map)
                                     .ToList();
        }

        private SquadFactoryDto Map(SquadContollerDto dto)
        {
            return new SquadFactoryDto
            {
                Type = dto.Type,
                Quantity = dto.Quantity ?? 0,
                Weapons = dto.Weapons?.ToList(),
                Armors = dto.Armors?.ToList()
            };
        }

        private async Task CreateArmyAsync()
        {
            army = await armyFactory.BuildAsync(factoryData);
        }

        private async Task SaveArmyAsync()
        {
            await army.SaveAsync(armyRepository);
        }

        private async Task NotifyThatCreatedAsync()
        {
            await army.NotifyThatCreatedAsync(armyNotificationsService);
        }

        private void LogThatCreated()
        {
            logger.Information($"Army for user {army.OwnerLogin} is created");
        }
    }
}
