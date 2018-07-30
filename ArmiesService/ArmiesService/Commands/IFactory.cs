using ArmiesService.Consumers.Data;
using ArmiesService.Controllers.Data;

namespace ArmiesService.Commands
{
    public interface IFactory
    {
        ICommand GetCreateArmyCommand(ArmyDto data);

        ICommand GetCreateUserCommand(UserDto data);
    }
}
