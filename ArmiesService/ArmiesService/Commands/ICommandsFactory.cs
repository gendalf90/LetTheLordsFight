using ArmiesService.Consumers.Data;
using ArmiesService.Controllers.Data;

namespace ArmiesService.Commands
{
    public interface ICommandsFactory
    {
        ICommand GetCreateArmyCommand(ArmyControllerDto data);

        ICommand GetCreateUserCommand(UserConsumerDto data);
    }
}
