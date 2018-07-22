using System.Threading.Tasks;

namespace ArmiesService.Common.CachingOperations
{
    interface IGetEntityStrategy<T> where T : class
    {
        Task<T> GetAsync();
    }
}
