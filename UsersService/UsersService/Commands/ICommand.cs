using System.Threading.Tasks;

namespace UsersService.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }
}
