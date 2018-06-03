using System.Threading.Tasks;

namespace ArmiesService.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }
}
