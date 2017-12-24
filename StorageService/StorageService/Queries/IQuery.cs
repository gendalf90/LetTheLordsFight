using System.Threading.Tasks;

namespace StorageService.Queries
{
    public interface IQuery<T>
    {
        Task<T> AskAsync();
    }
}
