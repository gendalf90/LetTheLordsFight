using System.Threading.Tasks;

namespace StorageService.Queries
{
    public interface IQuery
    {
        Task<string> AskAsync();
    }
}
