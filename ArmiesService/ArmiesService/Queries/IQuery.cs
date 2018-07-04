using System.Threading.Tasks;

namespace ArmiesService.Queries
{
    public interface IQuery<T>
    {
        Task<T> AskAsync();
    }
}
