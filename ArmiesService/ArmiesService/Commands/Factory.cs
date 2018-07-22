using IArmyFactory = ArmiesDomain.Factories.Armies.IFactory;
using ICommandFactory = ArmiesService.Commands.IFactory;
using ArmiesService.Logs;
using ArmyDtoOfController = ArmiesService.Controllers.Data.ArmyDto;
using ArmiesDomain.Repositories.Armies;
using CreateArmyCommand = ArmiesService.Commands.CreateArmy.Command;
using ArmiesService.Common;

namespace ArmiesService.Commands
{
    class Factory : ICommandFactory
    {
        private readonly IArmyFactory armyFactory;
        private readonly IArmies armyRepository;
        private readonly IGetCurrentUserLoginStrategy currentUserLogin;
        private readonly ILog logger;

        public Factory(IArmyFactory armyFactory,
                       IArmies armyRepository,
                       IGetCurrentUserLoginStrategy currentUserLogin,
                       ILog logger)
        {
            this.armyFactory = armyFactory;
            this.armyRepository = armyRepository;
            this.currentUserLogin = currentUserLogin;
            this.logger = logger;
        }

        public ICommand GetCreateArmyCommand(ArmyDtoOfController data)
        {
            return new CreateArmyCommand(armyFactory, armyRepository, currentUserLogin, logger, data);
        }
    }
}
