using ArmiesService.Logs;
using ArmiesDomain.Repositories.Armies;
using ArmiesService.Common;
using ArmiesDomain.Repositories.Users;
using ArmiesDomain.Services.ArmyNotifications;
using ArmiesDomain.Factories.Armies;
using ArmiesService.Controllers.Data;
using ArmiesService.Consumers.Data;
using ArmiesService.Commands.CreateArmy;
using ArmiesService.Commands.CreateUser;

namespace ArmiesService.Commands
{
    class CommandsFactory : ICommandsFactory
    {
        private readonly IArmyFactory armyFactory;
        private readonly IArmies armyRepository;
        private readonly IUsers usersRepository;
        private readonly IGetCurrentUserLoginStrategy currentUserLogin;
        private readonly IArmyNotificationService armyNotificationsService;
        private readonly ILog logger;

        public CommandsFactory(IArmyFactory armyFactory,
                               IArmies armyRepository,
                               IUsers usersRepository,
                               IGetCurrentUserLoginStrategy currentUserLogin,
                               IArmyNotificationService armyNotificationsService,
                               ILog logger)
        {
            this.armyFactory = armyFactory;
            this.armyRepository = armyRepository;
            this.usersRepository = usersRepository;
            this.currentUserLogin = currentUserLogin;
            this.armyNotificationsService = armyNotificationsService;
            this.logger = logger;
        }

        public ICommand GetCreateArmyCommand(ArmyControllerDto data)
        {
            return new CreateArmyCommand(armyFactory, armyRepository, currentUserLogin, armyNotificationsService, logger, data);
        }

        public ICommand GetCreateUserCommand(UserConsumerDto data)
        {
            return new CreateUserCommand(usersRepository, logger, data);
        }
    }
}
