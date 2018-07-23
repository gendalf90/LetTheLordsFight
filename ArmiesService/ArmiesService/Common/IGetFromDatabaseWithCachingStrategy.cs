using System.Threading.Tasks;

namespace ArmiesService.Common
{
    interface IGetFromDatabaseWithCachingStrategy
    {
        Task<T> GetAsync<T>(SearchParams searchParams) where T : class;
    }
}
