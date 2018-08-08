using ArmiesService.Consumers.Data;
using ArmiesService.Controllers.Data;

namespace ArmiesService.Commands
{
    public interface ICommandsFactory
    {
        ICommand GetCreateArmyCommand(ArmyPostDto data);

        ICommand GetCreateUserCommand(UserCreatedEventDto data);
    }
}
