using IArmyFactory = ArmiesDomain.Factories.Armies.IFactory;
using ICommandFactory = ArmiesService.Commands.IFactory;
using ArmiesService.Logs;
using ArmyDtoOfController = ArmiesService.Controllers.Data.ArmyDto;
using UserDtoOfConsumer = ArmiesService.Consumers.Data.UserDto;
using ArmiesDomain.Repositories.Armies;
using CreateArmyCommand = ArmiesService.Commands.CreateArmy.Command;
using CreateUserCommnad = ArmiesService.Commands.CreateUser.Command;
using ArmiesService.Common;
using ArmiesService.Consumers.Data;
using ArmiesDomain.Repositories.Users;

namespace ArmiesService.Commands
{
    class Factory : ICommandFactory
    {
        private readonly IArmyFactory armyFactory;
        private readonly IArmies armyRepository;
        private readonly IUsers usersRepository;
        private readonly IGetCurrentUserLoginStrategy currentUserLogin;
        private readonly ILog logger;

        public Factory(IArmyFactory armyFactory,
                       IArmies armyRepository,
                       IUsers usersRepository,
                       IGetCurrentUserLoginStrategy currentUserLogin,
                       ILog logger)
        {
            this.armyFactory = armyFactory;
            this.armyRepository = armyRepository;
            this.usersRepository = usersRepository;
            this.currentUserLogin = currentUserLogin;
            this.logger = logger;
        }

        public ICommand GetCreateArmyCommand(ArmyDtoOfController data)
        {
            return new CreateArmyCommand(armyFactory, armyRepository, currentUserLogin, logger, data);
        }

        public ICommand GetCreateUserCommand(UserDtoOfConsumer data)
        {
            return new CreateUserCommnad(usersRepository, logger, data);
        }
    }
}
