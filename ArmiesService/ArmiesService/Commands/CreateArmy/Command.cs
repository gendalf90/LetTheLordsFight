using ArmiesDomain.Factories.Armies;
using ArmiesDomain.Repositories.Armies;
using IArmyFactory = ArmiesDomain.Factories.Armies.IFactory;
using ArmiesService.Logs;
using ArmyDtoOfController = ArmiesService.Controllers.Data.ArmyDto;
using SquadDtoOfController = ArmiesService.Controllers.Data.SquadDto;
using System.Linq;
using System.Threading.Tasks;
using ArmiesDomain.Entities;

namespace ArmiesService.Commands.CreateArmy
{
    class Command : ICommand
    {
        private readonly IArmyFactory armyFactory;
        private readonly IArmies armyRepository;
        private readonly IGetCurrentUserLoginStrategy currentUserLogin;
        private readonly ILog logger;
        private readonly ArmyDtoOfController data;

        private ArmyData factoryData;
        private Army army;

        public Command(IArmyFactory armyFactory,
                       IArmies armyRepository,
                       IGetCurrentUserLoginStrategy currentUserLogin,
                       ILog logger,
                       ArmyDtoOfController data)
        {
            this.armyFactory = armyFactory;
            this.armyRepository = armyRepository;
            this.currentUserLogin = currentUserLogin;
            this.logger = logger;
            this.data = data;
        }

        public async Task ExecuteAsync()
        {
            CreateFactoryData();
            await CreateArmyAsync();
            await SaveArmyAsync();
            LogThatCreated();
        }

        private void CreateFactoryData()
        {
            factoryData = new ArmyData();
            factoryData.OwnerLogin = currentUserLogin.Get();
            factoryData.Squads = data.Squads?
                                     .Select(Map)
                                     .ToList();
        }

        private SquadData Map(SquadDtoOfController dto)
        {
            return new SquadData
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

        private void LogThatCreated()
        {
            logger.Information($"Army for user {army.OwnerLogin} is created");
        }
    }
}
