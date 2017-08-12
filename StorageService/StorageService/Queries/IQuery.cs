using System.Threading.Tasks;

namespace StorageService.Queries
{
    interface IQuery
    {
        Task<string> AskAsync();
    }
}
